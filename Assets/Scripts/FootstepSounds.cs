using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class FootstepSounds : MonoBehaviour
{
    //gcheck
    public LayerMask groundLayers;
    [SerializeField] private float _groundCheckRadius = 0.32f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private float _maxDistance = 0.22f;
    private Vector3 _groundCheckOrigin;
    private RaycastHit _groundInfo;
    public AudioSource _audioSource;

    [Tooltip("Should terrain sounds be blended when the player is on multiple textures at the same time")]
    public bool BlendTerrainSounds;
    public float FootstepAudioVolume;

    public AudioClip _footstepDecided;

    public bool fromFootstep;

    [Tooltip("Choose the textures, and texture sounds, both for footsteps and for landing")]
    [SerializeField] private TextureSound[] TextureSounds;

    
    public bool texInList;
    public AudioClip[] NullClips;
    //references
    public CharacterController _charController;

    void OnEnable()
    {
        AnimationEventReciever.OnFootstep += Footstep;
        AnimationEventReciever.OnLandStep += LandStep;
    }
    void OnDisable()
    {
        AnimationEventReciever.OnFootstep -= Footstep;
        AnimationEventReciever.OnLandStep -= LandStep;  
    }

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Footstep()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f *_charController.height + 0.5f * _charController.radius, 0), Vector3.down, out _groundInfo, 2f, groundLayers))
        {
            if (_groundInfo.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                fromFootstep = true;
                StartCoroutine(PlayFootstepFromRenderer(renderer));
                texInList = false;
                _audioSource.PlayOneShot(_footstepDecided, FootstepAudioVolume);
            }
            else if (_groundInfo.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                fromFootstep = true;
                StartCoroutine(PlayFootstepSoundFromTerrain(terrain, _groundInfo.point));
                texInList = false;
                if (!BlendTerrainSounds)
                {
                    _audioSource.PlayOneShot(_footstepDecided, FootstepAudioVolume);
                }
            }
        }
        
    }
    private void LandStep()
    {
        Debug.Log("cuistepped");
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f * _charController.height + 0.5f * _charController.radius, 0), Vector3.down, out _groundInfo, 2f, groundLayers))
        {
            if (_groundInfo.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                fromFootstep = false;
                StartCoroutine(PlayFootstepFromRenderer(renderer));
                texInList = false;
                _audioSource.PlayOneShot(_footstepDecided, FootstepAudioVolume);
            }
            else if (_groundInfo.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                StartCoroutine(PlayFootstepSoundFromTerrain(terrain, _groundInfo.point));
                texInList = false;
                _audioSource.PlayOneShot(_footstepDecided, FootstepAudioVolume);
            }
        }

    }
    private IEnumerator PlayFootstepSoundFromTerrain(Terrain Terrain, Vector3 HitPoint)
    {
        Vector3 terrainPosition = HitPoint - Terrain.transform.position;
        Vector3 splatMapPosition = new Vector3(
            terrainPosition.x / Terrain.terrainData.size.x,
            0,
            terrainPosition.z / Terrain.terrainData.size.z
        );

        int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);

        float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

        if (!BlendTerrainSounds)
        {
            int primaryIndex = 0;
            for (int i = 1; i < alphaMap.Length; i++)
            {
                if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])
                {
                    primaryIndex = i;
                }
            }

            foreach (TextureSound textureSound in TextureSounds)
            {
                
                if (textureSound.Albedo == Terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
                {
                    if (fromFootstep)
                    {
                        Debug.Log("cui");
                        _footstepDecided = GetClipFromTextureSound(textureSound, true);
                    }
                    else
                    {
                        _footstepDecided = GetClipFromTextureSound(textureSound, false);

                    }
                    yield return null;
                    break;
                }
            }
        }
        else
        {
            List<AudioClip> clips = new List<AudioClip>();
            int clipIndex = 0;
            for (int i = 0; i < alphaMap.Length; i++)
            {
                if (alphaMap[0, 0, i] > 0)
                {
                    foreach (TextureSound textureSound in TextureSounds)
                    {
                        if (textureSound.Albedo == Terrain.terrainData.terrainLayers[i].diffuseTexture)
                        {
                            if(fromFootstep)
                            {
                                AudioClip clip = GetClipFromTextureSound(textureSound, true);
                                _audioSource.PlayOneShot(clip, alphaMap[0, 0, i]);
                                clips.Add(clip);
                                clipIndex++;
                                break;
                            }
                            else
                            {
                                AudioClip clip = GetClipFromTextureSound(textureSound, false);
                                _audioSource.PlayOneShot(clip, alphaMap[0, 0, i]);
                                clips.Add(clip);
                                clipIndex++;
                                break;
                            }
                        }
                    }
                }
            }

            float longestClip = clips.Max(clip => clip.length);

                yield return new WaitForSeconds(longestClip);
            }
        

    }
    IEnumerator PlayFootstepFromRenderer(Renderer Renderer)
    {
        foreach (TextureSound textureSound in TextureSounds)
        {
            if (textureSound.Albedo == Renderer.material.GetTexture("_MainTex"))
            {
                texInList = true;
                if (fromFootstep)
                {
                    _footstepDecided = GetClipFromTextureSound(textureSound, true);
                }
                else
                {
                    _footstepDecided = GetClipFromTextureSound(textureSound, false);
                }

                yield return null;
                //texInList = false;
                break;
            }
        }
        if (!texInList)
        {
            _footstepDecided = GetClipNullClips();
            Debug.Log("PlayFootstepFromRenderer no texture");

            //audioSource.PlayOneShot(clip);
            yield return null; 
        }
        
    }

    private AudioClip GetClipFromTextureSound(TextureSound TextureSound, bool footStep)
    {
        if (footStep)
        {
            return GetClipFrom(TextureSound.Clips);
        }
        else
        {
            return GetClipFrom(TextureSound.JumpClips);
        }
        
    }
    private AudioClip GetClipNullClips()
    {
        return GetClipFrom(NullClips);
    }
    private AudioClip GetClipFrom(AudioClip[] clips)
    {
        int clipIndex = Random.Range(1, clips.Length);

        AudioClip temporary = clips[clipIndex];
        clips[clipIndex] = clips[0];
        clips[0] = temporary;

        return temporary;

    }

    [System.Serializable]
    private class TextureSound
    {
        public Texture Albedo;
        public AudioClip[] Clips;
        public AudioClip[] JumpClips;

    }
}


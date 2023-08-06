using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    //gcheck
    public LayerMask groundLayers;
    [SerializeField] private float _groundCheckRadius = 0.32f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private float _maxDistance = 0.22f;
    private Vector3 _groundCheckOrigin;
    private RaycastHit _groundInfo;

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

    void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f * _charController.height + 0.5f * _charController.radius, 0), Vector3.down, Color.blue);
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
                AudioSource.PlayClipAtPoint(_footstepDecided, transform.TransformPoint(_charController.center), FootstepAudioVolume);
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
                AudioSource.PlayClipAtPoint(_footstepDecided, transform.TransformPoint(_charController.center), FootstepAudioVolume);
            }
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
        Debug.Log("GetClipFrom");
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


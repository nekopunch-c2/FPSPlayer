using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NekoSpace;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    private float velocityCrouch;
    private Vector3 _movementSmooth;


    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) 
    {
        _isRootState = true;
        _movementSmooth = _ctx.MovementSmooth;
    }

    public override void EnterState()
    {
        _ctx.IsJumping = false;
        _ctx.IsFalling = false;
        _ctx.GroundedTimer = 0f;
        
        HandleGravity();
        InitializeSubState();
    }

    public override void UpdateState()
    {
        
        if(CheckSwitchStates())
        {
            return;
        }
        _ctx.Movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        //Debug.Log("Is falling equals " + _ctx.IsFalling);
        OnSlope();
        OutOfCrouch();
        HandleAnim();
        MovingPlatform();
        //HandleGravity();
        
        //_ctx.IsJumping = true;
    }

    public void HandleGravity()
    {
        _ctx.VelocityY = _ctx.Gravity * _ctx.GroundedGravity;
    }

    public override void ExitState()
    {

        _ctx.Animator.SetBool(_ctx.Turning, false);
        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, false);
        _ctx.Animator.SetBool(_ctx.AnimIDLanding, false);
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.PlayerJump && !_ctx.IsFalling)
        {

            SwitchState(_factory.Jump());
            return true;
        }
        else if (_ctx.GetPlayerClimb && (_ctx.AboveLadder || _ctx.BelowLadder) && !_ctx.IsFalling)
        {
            SwitchState(_factory.LadderTransition());
            return true;
        }
        else if (!_ctx.IsGrounded && !_ctx.IsJumping && !_ctx.OnStairsTagGetSet && !OnSlope())
        {
            //_ctx.IsJumping = true;
            _ctx.VelocityY = 0f;
            SwitchState(_factory.InAir());
            return true;
        }
        else if (_ctx.IsGrounded && _ctx.IsCrouching)
        {
            SwitchState(_factory.Crouch());
            return true;
        }
        return false;
    }

    public override void InitializeSubState()
    {
        if (!_ctx.IsMoving && !_ctx.IsRunning)
        {
            SetSubState(_factory.Idle());
        }
        else if (_ctx.IsMoving && (!_ctx.IsRunning))
        {
            SetSubState(_factory.Walk());
        }
        else
        {
            
            SetSubState(_factory.Run());
        }
    }

    private bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(_ctx.PlayerBody.position * (_ctx.CharacterController.height / 2), Vector3.down, out hit, _ctx.CharacterController.height / 2 * _ctx.SlopeForceRayLength) )
        {
            if (hit.normal != Vector3.up && _ctx.IsGrounded)
            {
                return true;
            }
        }
        return false;
    }

    private bool OnStairsTag()
    {

        RaycastHit hit;

        if (Physics.Raycast(_ctx.PlayerBody.position * (_ctx.CharacterController.height / 2), Vector3.down, out hit, _ctx.StairsCheckLength))
        {
            _ctx.CurrentGround = hit.transform;
            if (!_ctx.IsJumping && _ctx.CurrentGround.tag == "Stairs")
            {
                Debug.Log(_ctx.CurrentGround);
                return true;
            }
        }
        return false;
    }

    void OutOfCrouch()
    {
        float standing = Mathf.SmoothDamp(_ctx.CharacterController.height, _ctx.StandHeight, ref velocityCrouch, _ctx.CrouchSmoothTime);
        _ctx.CharacterController.height = standing;
        Vector3 standingCenter = Vector3.SmoothDamp(_ctx.CharacterController.center, _ctx.StandCenterAdjusted, ref _movementSmooth, _ctx.CrouchSmoothTime);
        _ctx.CharacterController.center = standingCenter;
    }
    void HandleAnim()
    {
        Debug.Log(_ctx.RotationDirection);
        if (_ctx.InputAllowed)
        {
            _ctx.Animator.SetFloat(_ctx.XVelHash, _ctx.CurrentInputVectorX);
            _ctx.Animator.SetFloat(_ctx.YVelHash, _ctx.CurrentInputVectorY);
        }
        if (_ctx.InputAllowed)
        {
            if (_ctx.RotationDirection != CameraRotationDirection.None && _ctx.GetPlayerMovement == Vector2.zero)
            {
                if (_ctx.RotationDirection == CameraRotationDirection.Left)
                {
                                         //Debug.Log("ciib");
                    _ctx.Animator.SetBool(_ctx.Turning, true);
                    _ctx.Animator.SetFloat(_ctx.TurningAngleHash, _ctx.RotationSpeed * -1f);
                }
                else if (_ctx.RotationDirection == CameraRotationDirection.Right)
                {
                    _ctx.Animator.SetBool(_ctx.Turning, true);
                    _ctx.Animator.SetFloat(_ctx.TurningAngleHash, _ctx.RotationSpeed);
                }

            }
            else
            {
                _ctx.Animator.SetBool(_ctx.Turning, false);
            }
            

        }
        //Debug.Log(_ctx.RotationSpeed);
        

        _ctx.Animator.SetBool(_ctx.AnimIDInAir, false);
        _ctx.Animator.SetBool(_ctx.AnimIDLanding, true);

        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, true);
        _ctx.Animator.SetBool(_ctx.AnimIDCrouching, false);
    }
    void MovingPlatform()
    {
        // If the player is standing on a moving platform, parent the player to the platform.
        if (_ctx.ActivePlatform != null)
        {
            _ctx.SetParent(_ctx.ActivePlatform);
        }
    }




}

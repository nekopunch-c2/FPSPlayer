using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _ctx.VelocityY = _ctx.Gravity * 0.1f;
        InitializeSubState();
    }

    public override void UpdateState()
    {
        Debug.Log("grouned");
        if(CheckSwitchStates())
        {
            return;
        }
        _ctx.Movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        Debug.Log("Is falling equals " + _ctx.IsFalling);
        OnSlope();
        OutOfCrouch();
        HandleAnim();
        //HandleGravity();
        HandleGravity();
        //_ctx.IsJumping = true;
    }

    public void HandleGravity()
    {
        if(_ctx.OnStairsTagGetSet || OnSlope())
            _ctx.VelocityY = _ctx.Gravity * _ctx.GroundedGravity;
    }

    public override void ExitState()
    {
        _ctx.VelocityY = 0f;
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.PlayerJump() && !_ctx.IsFalling)
        {

            SwitchState(_factory.Jump());
            return true;
        }
        else if (!_ctx.IsGrounded && !_ctx.IsJumping && _ctx.IsFalling)
        {
            //_ctx.IsJumping = true;
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
        if (!_ctx.IsMoving && (!_ctx.IsRunning))
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

        if (Physics.Raycast(_ctx.PlayerBody.position * (_ctx.CharacterController.height / 2), Vector3.down, out hit, _ctx.CharacterController.height / 2 * _ctx.SlopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
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

        if (_ctx.InputAllowed)
        {
            _ctx.Animator.SetFloat(_ctx.XVelHash, _ctx.CurrentInputVectorX);
            _ctx.Animator.SetFloat(_ctx.YVelHash, _ctx.CurrentInputVectorY);
        }

        _ctx.Animator.SetBool(_ctx.AnimIDInAir, false);
        _ctx.Animator.SetBool(_ctx.AnimIDLanding, true);

        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, true);
        _ctx.Animator.SetBool(_ctx.AnimIDCrouching, false);
    }
    

}

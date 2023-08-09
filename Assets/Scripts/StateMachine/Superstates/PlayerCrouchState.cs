using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private Vector3 _movementSmooth;
    private float velocityCrouch;
    private bool _canGoUp;

    public PlayerCrouchState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
    {
        _isRootState = true;
        _movementSmooth = _ctx.MovementSmooth;
    }

    public override void EnterState()
    {
        HandleGravity();
        _ctx.CrouchSpeedMultiplier = .5f;
        _ctx.IsJumping = false;
        _ctx.IsFalling = false;
        _ctx.GroundedTimer = 0f;
        InitializeSubState();
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates())
        {
            return;
        }
        RoofCheck();
        HandleMovement();
        HandleCrouch();
        HandleAnim();
        //_ctx.IsJumping = true;
    }

    

    public override void ExitState()
    {

        _ctx.CrouchSpeedMultiplier = 1f;
        _ctx.Animator.SetBool(_ctx.AnimIDCrouching, false);
        _ctx.Animator.SetBool(_ctx.AnimIDLanding, false);
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.PlayerJump && _canGoUp && !_ctx.IsFalling)
        {
            SwitchState(_factory.Jump());
            return true;
        }
        
        else if (!_ctx.IsGrounded && !_ctx.IsJumping && _canGoUp && !_ctx.OnStairsTagGetSet && !OnSlope())
        {
            //_ctx.IsJumping = true;
            _ctx.VelocityY = 0f;
            SwitchState(_factory.InAir());
            return true;
        }
        else if (!_ctx.IsJumping && !_ctx.IsCrouching && _canGoUp)
        {
            SwitchState(_factory.Grounded());
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

    public void HandleGravity()
    {
        _ctx.VelocityY += _ctx.Gravity * _ctx.GroundedGravity;
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
            if (!_ctx.IsJumping && _ctx.CurrentGround.tag == "Stairs" && !_ctx.IsFalling)
            {
                //Debug.Log("is on stairs via tag");
                return true;
            }
        }
        return false;
    }
    void HandleMovement()
    {
        _ctx.Movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
    }
    void RoofCheck()
    {
        Ray ray = new Ray(_ctx.PlayerBody.position, _ctx.PlayerBody.up);
        if (Physics.Raycast(ray, out RaycastHit roofCheckInfo, _ctx.RoofCheckDistance, _ctx.RoofLayers, QueryTriggerInteraction.Ignore))
        {
            _canGoUp = false;
            Debug.DrawRay(_ctx.PlayerBody.position, _ctx.PlayerBody.up * _ctx.RoofCheckDistance);
        }
        else
        {
            _canGoUp = true;
        }

    }

    void HandleCrouch()
    {
        _ctx.CharacterController.height = Mathf.SmoothDamp(_ctx.CharacterController.height, _ctx.CrouchHeight, ref velocityCrouch, _ctx.CrouchSmoothTime);

        _ctx.CharacterController.center = Vector3.SmoothDamp(_ctx.CharacterController.center, _ctx.CrouchCenterAdjusted, ref _movementSmooth, _ctx.CrouchSmoothTime);
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

        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, false);
        _ctx.Animator.SetBool(_ctx.AnimIDCrouching, true);
    }


}

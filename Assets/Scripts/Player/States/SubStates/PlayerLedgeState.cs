using Base.FSM;
using UnityEngine;
using System.Collections;

public class PlayerLedgeState : PlayerState
{
    private Vector2 _ledgeGrabPos;
    public bool Exiting { get; private set; }
    
    public PlayerLedgeState(Player player, PlayerDataSO data, string animBoolName, StateMachine stateMachine) :
        base(player, data, animBoolName, stateMachine) { }


    public override void Enter()
    {
        base.Enter();
        core.Movement.SetVelocity(Vector2.zero);
        
        _ledgeGrabPos = new Vector2(core.Detection.LedgePosition.x + core.Movement.FaceDirection * data.LedgeClimbOffset.x,
            core.Detection.LedgePosition.y + data.LedgeClimbOffset.y);
        
        player.transform.position = core.Detection.LedgePosition;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine(LedgeClimbExit(0.3f));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core.Movement.SetVelocity(Vector2.zero);
        player.transform.position = _ledgeGrabPos;

        if (player.InputHandler.JumpInput.Press)
            stateMachine.ChangeState(player.JumpState);
        else if (player.InputHandler.NormInputY < 0)
            stateMachine.ChangeState(player.AirState);
    }

    private IEnumerator LedgeClimbExit(float timer)
    {
        Exiting = true;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        Exiting = false;
    }
}

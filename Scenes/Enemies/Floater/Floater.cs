using Godot;
using System;

public class Floater : KinematicBody2D
{
    [Export]
    private float _speed = 20;                                          // The speed of the floater
    [Export]
    private Vector2 _maxSpeed = new Vector2(100, 0);                    // Max speed for the floater
    private AnimatedSprite _animations;                                 // The floater sprite animations
    private  bool _turning = false;                                     // If the floater is turning
    private Vector2 _velocity = Vector2.Zero;                           // The velocity of the floater
    public override void _Ready()
    {
        _animations = GetNode<AnimatedSprite>("AnimatedSprite");        // Get the sprite animated sprite node
    }

    private void ResetTurningIfNeeded()
    {
        // If the floater is turning
        if(_turning)
        {
            _velocity.x = 0;    // Reset the velocity to 0
            _turning = false;   // Flag that the floater no longer is turning
        }
    }    

    private void OnAnimatedSpriteAnimationFinished()
    {
        ResetTurningIfNeeded();     // Reset turning of the floater if needed        
        _animations.Play("Fly");    // Play the fly animation
    }

    private void UpdateMove()
    {
        // If the animation is flipped (going left) 
        // and the floater is not currently turning
        if(_animations.FlipH && !_turning)
        {
            _velocity -= new Vector2(1, 0);     // Speed up the floater

            // If the floater has passed it's maximum speed to the left
            if(_velocity.x < -_maxSpeed.x)
            {
                _velocity = -_maxSpeed;         // Set the velocity to the maximum speed
            }
        }
        if(!_animations.FlipH && !_turning)
        {
            _velocity += new Vector2(1, 0);     // Speed up the floater

            // If the floater has passed it's maximum speed to the right
            if(_velocity.x > _maxSpeed.x)
            {
                _velocity = _maxSpeed;          // Set the velocity to the maximum speed
            }            
        }

        // Run the MoveAndSlide() method to move the floater with Godot's in-built physics
        MoveAndSlide(_velocity,Vector2.Up,false);
    }

    private void TurnOnCollision()
    {
        var count = GetSlideCount();    // Get the number of collisions
        
        // If the floater has more than one collision and is not turning
        if(count > 0 && !_turning)
        {
            _turning = true;                        // Set turning to true
            _animations.Play("Turn");               // Play the turning animation
            _animations.FlipH = !_animations.FlipH; // Flip the sprite
        }
    }

    public override void _Process(float delta)
    {
        UpdateMove();       // Move the floater
        TurnOnCollision();  // Turn the floater on collision
    }
}

# Unity Character Movement System Documentation

## Overview
This document provides a comprehensive overview of the character movement system implemented in Unity. The system is designed with a modular architecture, separating different movement functionalities into distinct components that work together to create a responsive character controller.

## Core Architecture

The movement system is built around the following key scripts:

1. **CharacterCore**: Central coordinator that connects all subsystems
2. **InputHandler**: Processes raw player input
3. **MovementSystem**: Handles horizontal movement and character flipping
4. **JumpSystem**: Manages jumping mechanics with coyote time and jump buffering
5. **GroundSensor**: Detects when the character is touching the ground
6. **AnimationSystem**: Controls animation states and transitions
7. **StaminaSystem**: Manages character stamina resources
8. **SurgeSystem**: Implements a speed boost ability using stamina

## System Breakdown

### CharacterCore.cs
The central orchestrator that connects all subsystems and manages their interaction.

**Key Responsibilities:**
- Acts as the main hub connecting all character subsystems
- Updates movement based on input
- Triggers jump attempts
- Updates animation states based on movement and grounding

**Dependencies:**
- InputHandler
- Rigidbody
- MovementSystem
- GroundSensor
- JumpSystem
- AnimationSystem

**Usage Example:**
```csharp
// Core update loop that coordinates subsystems
void Update()
{
    movement.Move(_inputHandler.MoveInput);
    _jumpSystem.TryJump();
    _animationSystem.UpdateGroundedState(groundSensor.IsGrounded);
    _animationSystem.UpdateWalkingState(movement.IsMoving, movement.CurrentSpeed);
}
```

### InputHandler.cs
Processes raw player input and provides a clean API for other systems.

**Key Properties:**
- `MoveInput`: Horizontal movement input (-1 to 1)
- `JumpPressed`: Whether jump was pressed this frame
- `JumpHeld`: Whether jump button is being held

**Usage Example:**
```csharp
// In another system
float direction = _inputHandler.MoveInput;
if (_inputHandler.JumpPressed) {
    // Initiate jump
}
```

### MovementSystem.cs
Handles horizontal movement, character flipping, and movement speed modifications.

**Key Features:**
- Basic horizontal movement with variable speed
- Character flipping logic (with optional inversion)
- Temporary speed modification system
- Event system for flip notifications

**Public Properties:**
- `IsMoving`: Whether the character is currently moving
- `CurrentSpeed`: The actual movement speed magnitude
- `IsFacingRight`: Direction the character is facing
- `BaseMoveSpeed`: The base movement speed value

**Public Methods:**
- `Move(float direction)`: Moves character in the specified direction
- `SetTemporarySpeed(float multiplier)`: Temporarily modifies movement speed
- `ResetToBaseSpeed()`: Restores original movement speed

**Events:**
- `OnFlipped`: Simple event when character flips
- `OnCharacterFlipped`: Event with detailed flip data

**Usage Example:**
```csharp
// Basic movement
movementSystem.Move(horizontalInput);

// Speed boost
movementSystem.SetTemporarySpeed(1.5f);

// Reset speed
movementSystem.ResetToBaseSpeed();
```

### JumpSystem.cs
Manages jumping mechanics with coyote time and jump buffering for responsive controls.

**Key Features:**
- Jump force configuration
- Anticipation animation before jumping
- Coyote time (grace period to jump after leaving ledge)
- Jump buffering (pre-pressing jump before landing)

**Key Properties:**
- `jumpForce`: Vertical force applied during jump
- `anticipationTime`: Time for anticipation animation before jump
- `coyoteTime`: Time window to jump after leaving ground
- `jumpBufferTime`: Time window to buffer jump input before landing

**Public Methods:**
- `TryJump()`: Attempts to jump, respecting coyote time and buffer conditions

**Usage Example:**
```csharp
// Call this method continuously to attempt jumps
jumpSystem.TryJump();
```

### GroundSensor.cs
Detects when the character is in contact with the ground using physics.

**Key Features:**
- Ground detection using Physics.CheckSphere
- Visual debugging aid in Scene view

**Key Properties:**
- `IsGrounded`: Whether the character is currently on the ground

**Configuration:**
- `checkOrigin`: Transform position for ground check
- `checkRadius`: Radius of the ground check sphere
- `groundLayer`: LayerMask for ground objects

**Usage Example:**
```csharp
if (groundSensor.IsGrounded) {
    // Perform grounded actions
}
```

### AnimationSystem.cs
Controls character animations with smooth transitions and state management.

**Key Features:**
- Walk animation with smoothing and "stickiness"
- Jump, land and flip animation triggers
- Surge state animation management

**Public Methods:**
- `UpdateWalkingState(bool isWalking, float moveSpeed)`: Updates walking animation
- `UpdateGroundedState(bool isGrounded)`: Updates grounded animation parameter
- `TriggerAnticipation()`: Plays jump anticipation animation
- `TriggerJump()`: Plays jump animation
- `TriggerLand()`: Plays landing animation
- `TriggerFlip()`: Plays flip animation
- `TriggerWalkStart()`: Plays walk start animation
- `TriggerWalkRestart()`: Resets walk start animation state
- `SetSurging(bool isSurging)`: Sets surging animation state

**Usage Example:**
```csharp
// Update animation states
animationSystem.UpdateWalkingState(isMoving, currentSpeed);
animationSystem.UpdateGroundedState(isGrounded);

// Trigger specific animations
animationSystem.TriggerJump();
```

### StaminaSystem.cs
Manages character stamina as a resource for abilities like surge.

**Key Features:**
- Stamina pool with configurable maximum
- Automatic regeneration with adjustable rate
- Support for temporary regeneration rate modifiers
- Special handling for surge ability cost and penalties

**Public Properties:**
- `CurrentStamina`: Current stamina amount
- `MaxStamina`: Maximum stamina capacity
- `RegenRate`: Current stamina regeneration rate

**Public Methods:**
- `ModifyStamina(float amount)`: Increases or decreases stamina
- `ModifyRegenMultiplier(float multiplier)`: Changes regeneration rate
- `TryPaySurgeCost()`: Attempts to use stamina for surge ability
- `ApplySurgeRegenPenalty()`: Reduces regen rate during surge
- `ResetRegen()`: Restores normal regen rate

**Usage Example:**
```csharp
// Using stamina for an ability
if (staminaSystem.TryPaySurgeCost()) {
    // Activate ability
}

// Manually modify stamina
staminaSystem.ModifyStamina(-10f);
```

### SurgeSystem.cs
Implements a speed boost ability that consumes stamina.

**Key Features:**
- Temporary speed boost with configurable multiplier
- Stamina cost and regeneration penalty
- Input handling for activation/deactivation
- Auto-cancellation when stamina depletes

**Key Properties:**
- `IsSurging`: Whether surge is currently active
- `LockedFacingDirection`: Direction locked during surge

**Dependencies:**
- MovementSystem
- StaminaSystem
- AnimationSystem

**Usage Example:**
```csharp
// Handled internally through input, but can be triggered manually:
surgeSystem.StartSurge();
surgeSystem.EndSurge();
```

## Animation and Visual Enhancement Systems

### AnimationOverlayHandler.cs
Handles specialized animation transitions like walk start/stop to improve visual polish.

**Key Features:**
- Tracks idle time to trigger walk-start animation
- Ensures walk-start only plays after sufficient idle time

**Configuration:**
- `_idleTimeRequired`: How long character must be idle before walk-start animation plays

**Dependencies:**
- MovementSystem
- AnimationSystem

**Usage Example:**
```csharp
// Automatically tracks idle time and triggers animations
// Add as component to character
```

### FlipAdjuster.cs
Provides fine-grained control over attached objects during character flips.

**Key Features:**
- Adjusts position and rotation of child objects when character flips
- Maintains consistent offsets based on facing direction
- Event-based interaction with MovementSystem

**Configuration:**
- `_positionOffsetRight`: Position offset when facing right
- `_positionOffsetLeft`: Position offset when facing left
- `_rotationOffset`: Rotation offset when flipped

**Dependencies:**
- MovementSystem (subscribes to flip events)

**Usage Example:**
```csharp
// Add to child objects that need special handling during flips
// e.g., weapons, accessories, visual effects
```

### RandomFacialAnimations.cs
Adds life to character with procedural animation triggers.

**Key Features:**
- Randomized blink animations
- Configurable timing parameters
- Manual trigger option for scripted moments

**Configuration:**
- `_minWaitTime`: Minimum time between blinks
- `_maxWaitTime`: Maximum time between blinks
- `_blinkCooldown`: Cooldown to prevent rapid consecutive blinks

**Public Methods:**
- `ForceBlink()`: Manually trigger a blink animation

**Usage Example:**
```csharp
// Add to face/head animator
randomFacialAnimations.ForceBlink(); // Force a blink for reactions
```

## Mouse Control and Cursor System

### MouseController.cs
Manages mouse cursor interaction with character movement.

**Key Features:**
- Translates character position to UI space
- Handles soft lock zone for cursor movement
- Reacts to character flips with cursor offset

**Configuration:**
- `flipDecayRate`: How quickly flip-based cursor offsets decay
- References to character, cameras, and UI elements

**Dependencies:**
- MovementSystem (for flip events)
- CoordinateConverter
- SoftLockZone

**Usage Example:**
```csharp
// Add to UI controller that manages the cursor
// Configure references to character and canvas elements
```

### SoftLockZone.cs
Implements a dynamic cursor constraint system.

**Key Features:**
- Gradually dampens cursor movement as it approaches boundaries
- Provides comfortable movement zone with natural feeling constraints
- Prevents cursor from moving too far from character

**Configuration:**
- `softLockRadius`: Maximum distance cursor can move from lock center
- `dampingThreshold`: Distance at which movement damping begins

**Usage Example:**
```csharp
// Used by MouseController
softLockZone.ProcessMovement(currentPos, delta, lockCenter);
```

### CoordinateConverter.cs
Utility for converting between world space and UI canvas space.

**Key Features:**
- Static utility for coordinate space conversion
- Handles different canvas render modes

**Public Methods:**
- `WorldToCanvasPoint()`: Converts world position to canvas local position

**Usage Example:**
```csharp
Vector2 uiPos = CoordinateConverter.WorldToCanvasPoint(
    worldPos, mainCamera, canvasRect, uiCamera
);
```

## Debug and Development Tools

### StateDebugger.cs
Provides real-time visual feedback of system states during gameplay.

**Key Features:**
- On-screen display of critical system values
- Color-coded sections for easy reading
- Automatic reference gathering in editor

**Configuration:**
- Display settings for colors, position, and spacing
- References to all major systems

**Dependencies:**
- InputHandler
- MovementSystem
- GroundSensor
- AnimationSystem
- SurgeSystem
- StaminaSystem

**Usage Example:**
```csharp
// Add to character GameObject during development
// Remove or disable for production builds
```

## System Interactions

### Movement Flow
1. **InputHandler** processes raw input
2. **CharacterCore** passes input to MovementSystem
3. **MovementSystem** applies velocity to Rigidbody
4. **AnimationSystem** updates visuals based on movement state

### Jump Flow
1. **InputHandler** detects jump button press
2. **CharacterCore** calls JumpSystem.TryJump()
3. **JumpSystem** checks eligibility via GroundSensor
4. If conditions met, plays anticipation animation via AnimationSystem
5. After anticipation, applies jump force to Rigidbody

### Surge Flow
1. **InputHandler** (via direct key check in SurgeSystem) detects surge key
2. **SurgeSystem** requests stamina from StaminaSystem
3. If approved, increases speed via MovementSystem
4. Updates animation state via AnimationSystem
5. Reduces stamina regeneration rate
6. Monitors for key release or stamina depletion to end surge

### Animation Enhancement Flow
1. **AnimationOverlayHandler** tracks idle time
2. When movement starts after sufficient idle time, triggers walk-start animation
3. **RandomFacialAnimations** periodically triggers blink animations
4. **FlipAdjuster** responds to flip events to reposition attached objects

### Mouse Control Flow
1. **MouseController** converts character position to canvas space using **CoordinateConverter**
2. Processes raw mouse input and any flip-based offsets
3. Uses **SoftLockZone** to constrain cursor movement
4. Updates cursor UI position

## Extension Points

The modular architecture allows for easy extensions:

1. **Additional Abilities**: Create new system classes following the pattern of SurgeSystem
2. **Movement Modifiers**: Add components that modify MovementSystem parameters
3. **Environmental Effects**: Create systems that interact with GroundSensor or modify movement/jump parameters
4. **AI Control**: Replace InputHandler with an AI controller that provides the same outputs
5. **Visual Enhancements**: Add more specialized animation handlers like AnimationOverlayHandler
6. **UI Integration**: Extend the mouse control system for additional UI interactions

## Technical Notes

- The system uses Unity's built-in physics system with Rigidbody components
- Character flipping is achieved through scale manipulation rather than rotation
- Animation transitions use a combination of parameters and triggers
- Jump mechanics feature both anticipation animation and coyote time for responsive feel
- Surge ability demonstrates how to implement resource-based abilities
- Mouse control system shows coordinate space conversion between world and UI
- Debug tools facilitate development and testing

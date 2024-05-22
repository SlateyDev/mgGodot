// public partial class CameraBoomController : SpringArm3D
// {
//     private Camera3D _camera;
//
//     private float _cameraAngleV;
//     private float _cameraAngleH;
//     private float _cameraZoom;
//
//     private const float MouseSensitivity = 0.02f;
//     private readonly float MinAngleH = Mathf.DegToRad(-25);
//     private readonly float MaxAngleH = Mathf.DegToRad(-80);
//
//     private const float MoveSpeed = 10;
//     private const float TurboMoveSpeed = 100;
//
//     private Vector2? _captureLocation;
//     
//     //Occupier - Character or object that occupies the cell so when I click on a cell I am selecting the character/object
//     //Characters/objects should also reference the cell they are in so I can click on a character or object and know where it is.
//
//     public override void _Ready()
//     {
//         _camera = (Camera3D)GetNode("Camera3D");
//         Position = new Vector3(50, 0, 50);
//     }
//
//     public override void _Process(double delta)
//     {
//         var turbo = Input.IsActionPressed("turbo");
//         var esc = Input.IsActionPressed("ui_cancel");
//
//         if (esc)
//         {
//             GetTree().Quit();
//         }
//
//         var horizontal = Input.GetAxis("left", "right") * (float)delta;
//         var vertical = Input.GetAxis("forward", "back") * (float)delta;
//         Translate(new Vector3(horizontal * (turbo ? TurboMoveSpeed : MoveSpeed), 0,
//             vertical * (turbo ? TurboMoveSpeed : MoveSpeed)).Rotated(Vector3.Right, -Rotation.X));
//
//         Rotation += new Vector3(0, Mathf.DegToRad(_cameraAngleV), 0);
//
//         var newLookAngleRotation = Rotation + new Vector3(Mathf.DegToRad(_cameraAngleH), 0, 0);
//         if (newLookAngleRotation.X > MinAngleH)
//         {
//             newLookAngleRotation.X = MinAngleH;
//         }
//
//         if (newLookAngleRotation.X < MaxAngleH)
//         {
//             newLookAngleRotation.X = MaxAngleH;
//         }
//
//         Rotation = newLookAngleRotation;
//
//         SpringLength += _cameraZoom;
//
//         _cameraAngleV = 0;
//         _cameraAngleH = 0;
//         _cameraZoom = 0;
//     }
//
//     public override void _Input(InputEvent inputEvent)
//     {
//         if (inputEvent is InputEventMouseMotion inputEventMouseMotion)
//         {
//             if (_captureLocation != null)
//             {
//                 _cameraAngleV += -inputEventMouseMotion.Relative.X * MouseSensitivity;
//                 _cameraAngleH += inputEventMouseMotion.Relative.Y * MouseSensitivity;
//             }
//         }
//
//         if (inputEvent is not InputEventMouseButton inputEventMouseButton) return;
//         if (_captureLocation == null) return;
//         if ((inputEventMouseButton.ButtonMask & MouseButtonMask.Right) != 0) return;
//         Input.MouseMode = Input.MouseModeEnum.Visible;
//         GetViewport().WarpMouse(_captureLocation.Value);
//         _captureLocation = null;
//     }
//
//     public override void _UnhandledInput(InputEvent inputEvent)
//     {
//         switch (inputEvent)
//         {
//             case InputEventMouseButton inputEventMouseButton:
//                 switch (inputEventMouseButton.ButtonIndex)
//                 {
//                     case MouseButton.Right:
//                         if (inputEventMouseButton.Pressed)
//                         {
//                             _captureLocation = inputEventMouseButton.Position;
//                             Input.MouseMode = Input.MouseModeEnum.Captured;
//                         }
//                         else
//                         {
//                             if (_captureLocation != null)
//                             {
//                                 Input.MouseMode = Input.MouseModeEnum.Visible;
//                                 GetViewport().WarpMouse(_captureLocation.Value);
//                             }
//
//                             _captureLocation = null;
//                         }
//
//                         break;
//                     case MouseButton.WheelUp:
//                         if (!inputEventMouseButton.Pressed) break;
//                         if (Input.IsKeyPressed(Key.Ctrl))
//                         {
//                             EmitSignalAtPosition(inputEventMouseButton.Position, "MoveCellUp");
//                         }
//                         else
//                         {
//                             _cameraZoom -= 2;
//                         }
//
//                         break;
//                     case MouseButton.WheelDown:
//                         if (!inputEventMouseButton.Pressed) break;
//                         if (Input.IsKeyPressed(Key.Ctrl))
//                         {
//                             EmitSignalAtPosition(inputEventMouseButton.Position, "MoveCellDown");
//                         }
//                         else
//                         {
//                             _cameraZoom += 2;
//                         }
//
//                         break;
//                 }
//
//                 break;
//             case InputEventMouseMotion inputEventMouseMotion:
//                 if ((inputEventMouseMotion.ButtonMask & MouseButtonMask.Right) != 0)
//                 {
//                     _cameraAngleV += -inputEventMouseMotion.Relative.X * MouseSensitivity;
//                     _cameraAngleH += inputEventMouseMotion.Relative.Y * MouseSensitivity;
//                 }
//
//                 break;
//             case InputEventPanGesture inputEventPanGesture:
//                 if (Input.IsKeyPressed(Key.Ctrl))
//                 {
//                     if (inputEventPanGesture.Delta.Y > 0.1)
//                     {
//                         EmitSignalAtPosition(inputEventPanGesture.Position, "MoveCellUp");
//                     }
//                     else if (inputEventPanGesture.Delta.Y < -0.1)
//                     {
//                         EmitSignalAtPosition(inputEventPanGesture.Position, "MoveCellDown");
//                     }
//                 }
//                 else
//                 {
//                     _cameraZoom += -inputEventPanGesture.Delta.Y;
//                 }
//                 break;
//         }
//     }
//
//     private Dictionary CastRayToPosition(Vector2 position)
//     {
//         var from = _camera.GlobalPosition;
//         var to = from + _camera.ProjectRayNormal(position) * 1000;
//         var spaceState = GetWorld3D().DirectSpaceState;
//         var result = spaceState.IntersectRay(new PhysicsRayQueryParameters3D { From = from, To = to });
//         return result;
//     }
//
//     private void EmitSignalAtPosition(Vector2 position, string signalName)
//     {
//         var result = CastRayToPosition(position);
//         if (result.Count <= 0) return;
//         var touchObject = GetNode(((Node)result["collider"]).GetPath());
//         var parentObject = touchObject.GetParent();
//         if (parentObject.Name == "HexMesh")
//         {
//             parentObject.GetParent().GetParent().EmitSignal(signalName, result["position"]);
//         }
//         else if (touchObject is Character character)
//         {
//             GD.Print("Action on character");
//             character.OccupyingCell.Chunk.GetParent().EmitSignal(signalName, result["position"]);
//         }
//     }
// }
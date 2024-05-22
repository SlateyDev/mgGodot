using System;
using System.Collections.Concurrent;
using Godot;
using Thread = System.Threading.Thread;

namespace HexadoniaApp.HexGrid;

public class ChunkBuilder : Node
{
    private bool _stillRunning;

     private readonly ConcurrentQueue<HexGridChunk> _chunkQueue = new();
     
     public ChunkBuilder()
     {
         _stillRunning = true;
         var builderThread = new Thread(BuilderProcess);
         builderThread.Start();
     }

     ~ChunkBuilder()
     {
         _stillRunning = false;
     }

     public void Shutdown()
     {
         _stillRunning = false;
     }

     public void QueueChunk(HexGridChunk chunk)
     {
         _chunkQueue.Enqueue(chunk);
     }

     private void BuilderProcess()
     {
         while (true)
         {
             if (!_stillRunning) return;
             
             while (_chunkQueue.TryDequeue(out var hexGridChunk))
             {
                 //TODO: Might be better if we can check for disposed items rather than getting an exception
                 try
                 {
                     hexGridChunk.Triangulate();
//TODO: Create collider after chunk build
//                     //For some reason, creating the collider while multithreading can crash the game
//                     hexGridChunk.CallDeferred("CreateCollider");
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Exception: {ex.Message}");
                 }
             }
         }
     }
}
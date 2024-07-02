using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassStageManager : IStageManager
{
   public event Action StageCompleted;

   public void Initialize()
   {
      Debug.Log("Initialize Glass stage");
   }

   public void Cleanup()
   {
      StageCompleted?.Invoke();
   }
}

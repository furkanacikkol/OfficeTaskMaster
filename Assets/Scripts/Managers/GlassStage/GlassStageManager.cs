using System;
using DG.Tweening;
using UnityEngine;

public class GlassStageManager : IStageManager
{
   public event Action StageCompleted;
   public bool GlassOnWaterDrinker { get; private set; }
   public bool IsGlassFilled { get; private set; }
   public bool IsPlantWatered { get; private set; }

   private Glass _glass;
   public void Initialize()
   {
      Debug.Log("Initialize Glass stage");
   }

   public void Cleanup()
   {
      StageCompleted?.Invoke();
      _glass?.Initialize();
   }
   
   public void GlassPlacedOnWaterDrinker(Glass glass)
   {
      GlassOnWaterDrinker = true;
      _glass = glass;
      Debug.Log("Glass placed on water drinker. Glass stage progressing...");

   }

   public void HandleWaterDrinkerClicked()
   {
      if (!GlassOnWaterDrinker) return;
      
      _glass.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
      IsGlassFilled = true;
      
      Cleanup();
   }

   public void PlantWatered(Transform glassPosition)
   {
      _glass.transform.DOMove(glassPosition.position, 1);
      _glass.transform.DORotate(glassPosition.rotation.eulerAngles, 1);

      IsPlantWatered = true;
      
      Cleanup();
   }
}

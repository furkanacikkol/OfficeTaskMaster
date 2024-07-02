using UnityEngine;
using Zenject;

public class GlassStageInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GlassStageManager>().AsSingle();
    }
}
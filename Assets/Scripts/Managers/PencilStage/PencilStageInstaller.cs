using UnityEngine;
using Zenject;

public class PencilStageInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PencilStageManager>().FromComponentInHierarchy().AsSingle();
    }
}
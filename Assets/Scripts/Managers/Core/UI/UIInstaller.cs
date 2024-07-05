using Zenject;

public class UIInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
    }
}
namespace FC.Auto.Framework.Interface
{
    public interface IPage
    {
        void Refresh();
        void WaitForPageToLoad();

        void Navigate();
    }
}

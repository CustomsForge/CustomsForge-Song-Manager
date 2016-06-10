namespace CustomsForgeSongManager.DataObjects
{
    public interface INotifyTabChanged
    {
        void TabEnter();
        void TabLeave();
    }

    public interface INamedTabControl
    {
        string GetTabName();
    }

    public interface IDataGridViewHolder
    {
        System.Windows.Forms.DataGridView GetGrid();
    }

    public interface IControl
    {
        System.Windows.Forms.Control GetControl();
    }

    public interface IMainForm : IControl
    {
        //System.Windows.Forms.Control CurrentControl { get; set; }
        //NotifyIcon Notifier { get; }
    }
}
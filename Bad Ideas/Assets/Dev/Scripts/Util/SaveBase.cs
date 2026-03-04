using QFSW.QC;

public interface SaveBase
{
    public void SetData(string field, string data);
    public void SaveData();
    public void LoadData();
    public void ReadData();

}

public abstract class SaveVar 
{
    public string Version;
};
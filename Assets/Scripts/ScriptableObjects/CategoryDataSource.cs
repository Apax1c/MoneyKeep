using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CategoryDataSource", menuName = "CategoryDataSource")]
public class CategoryDataSource : DataTable<CategoryData>
{
    public override CategoryData GetConfigData(int id)
    {
        CategoryData result = lsItems.Where(data => data.id == id).FirstOrDefault();
        return result;
    }
}

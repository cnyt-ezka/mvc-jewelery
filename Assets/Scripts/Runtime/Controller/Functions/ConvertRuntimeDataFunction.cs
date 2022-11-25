using System.Text.RegularExpressions;
using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Data.ValueObject.Source;
using UnityEngine;

namespace Runtime.Controller.Functions
{
    public class ConvertRuntimeDataFunction<T> : MVCFunction<T, T> where T : BaseVO
    {
        public override T Execute(T vo)
        {
            var data = JsonUtility.ToJson(vo);

            var splitArray = Regex.Split(data, "IsRuntimeData");
            data = "";
            for (int i = 0; i < splitArray.Length; i++)
            {
                var item = splitArray[i];
                if (item.Length >= 7 && item.Substring(2, 5) == "false")
                {
                    item = item.Remove(2, 5);
                    item = item.Insert(2, "true");
                    item = item.Insert(0, "IsRuntimeData");
                    data += item;
                }
                else
                {
                    data += item;
                }
            }

            //Debug.Log("---Serilaze Data : --" + serializedData);
            var newArea = JsonUtility.FromJson<T>(data);
            //Debug.Log("FROMMJSON NEW AREA : " + newArea.IsRuntimeData);
            
            return newArea;
        }
    }
}

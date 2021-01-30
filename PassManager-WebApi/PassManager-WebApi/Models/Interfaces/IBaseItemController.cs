using System.Web.Http;

namespace PassManager_WebApi.Models.Interfaces
{
    public interface IBaseItemController<ItemType>
    {
        //GET api/ItemType
        IHttpActionResult Get();
        //GET api/ItemType?lastCreated=true
        IHttpActionResult Get(bool lastCreated);
        //GET api/ItemType/5
        IHttpActionResult Get(int id);
        //POST api/ItemType
        IHttpActionResult Post([FromBody] ItemType item);
        //PUT api/ItemType/5
        IHttpActionResult Put(int id, [FromBody] ItemType item);
        //DELETE api/ItemType/5
        IHttpActionResult Delete(int id);
    }
}
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPV.Data.DataItems;
using Microsoft.Extensions.Configuration;
using My.Projects.Web.Classes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace My.Projects.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TDataItem> : ControllerBase
        where TDataItem : DataItem, new()
    {
        private IConfiguration _configuration;
        private string ConnectionString { get; }

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
            if (Environment.MachineName == "KAR-GIT-2")
                ConnectionString = _configuration.GetConnectionString("KpProjectsConnection");
            else
                ConnectionString = _configuration.GetConnectionString("MyProjectsConnection");
                    
        }
        
        // GET: api/<BaseController>
        [HttpGet]
        public IList<TDataItem> Get()
        {
            MyProjectsConnection connection 
                = new MyProjectsConnection(ConnectionString);
            return connection.LoadList<TDataItem>();
        }

        // GET api/<BaseController>/5
        [HttpGet("{id}")]
        public TDataItem Get(Guid id)
        {
            MyProjectsConnection connection
                = new MyProjectsConnection(ConnectionString);
            TDataItem dataItem = new TDataItem {Id = id};
            connection.Load(dataItem);
            return dataItem;
        }

        // POST api/<BaseController>
        [HttpPost]
        public void Post([FromBody] TDataItem dataItem)
        {
            MyProjectsConnection connection
                = new MyProjectsConnection(ConnectionString);
            connection.Save(dataItem);
        }

        // PUT api/<BaseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BaseController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            TDataItem dataItem = new TDataItem {Id = id};

            MyProjectsConnection connection
                = new MyProjectsConnection(ConnectionString);
            connection.Delete(dataItem);
        }

        [HttpGet("[action]")]
        public IList<Reference<TDataItem>> LoadReferences()
        {
            MyProjectsConnection connection = new MyProjectsConnection(ConnectionString);
            return connection.LoaReferences<TDataItem>();
        }
    }
}

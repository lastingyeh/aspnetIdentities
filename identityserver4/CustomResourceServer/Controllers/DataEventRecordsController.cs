using System;
using CustomResourceServer.Models;
using CustomResourceServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomResourceServer.Controllers
{
    [Authorize("dataEventRecords")]
    [Route("api/[controller]")]
    public class DataEventRecordsController: ControllerBase
    {
        private readonly IDataEventRecordRepository _dataEventRecordRepository;
        public DataEventRecordsController(IDataEventRecordRepository dataEventRecordRepository)
        {
            _dataEventRecordRepository = dataEventRecordRepository;
        }

        [Authorize("dataEventRecordsUser")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dataEventRecordRepository.GetAll());
        }

        [Authorize("dataEventRecordsAdmin")]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_dataEventRecordRepository.Get(id));
        }

        [Authorize("dataEventRecordsAdmin")]
        [HttpPost]
        public void Post([FromBody]DataEventRecord value)
        {
            _dataEventRecordRepository.Post(value);
        }

        [Authorize("dataEventRecordsAdmin")]
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody]DataEventRecord value)
        {
            _dataEventRecordRepository.Put(id, value);
        }

        [Authorize("dataEventRecordsAdmin")]
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _dataEventRecordRepository.Delete(id);
        }
    }
}

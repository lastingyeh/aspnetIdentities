using System;
using System.Collections.Generic;
using System.Linq;
using CustomResourceServer.Data;
using CustomResourceServer.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

namespace CustomResourceServer.Repositories
{
    public class DataEventRecordRepository : IDataEventRecordRepository
    {
        private readonly ILogger<DataEventRecordRepository> _logger;
        private readonly IDataProtector _protector;
        private readonly DataEventRecordContext _context;

        public DataEventRecordRepository(DataEventRecordContext context, ILogger<DataEventRecordRepository> logger, IDataProtectionProvider provider)
        {
            _context = context;
            _protector = provider.CreateProtector("DataEventRecordRepository.v1");
            _logger = logger;
        }

        public DataEventRecord Get(Guid id)
        {
            var dataEventRecord = _context.DataEventRecords.First(t => t.Id == id);

            // unprotectDescription(dataEventRecord);

            return dataEventRecord;
        }

        public List<DataEventRecord> GetAll()
        {
            _logger.LogCritical("Getting a the existing records");
            
            var data =  _context.DataEventRecords.ToList();

            // foreach(var item in data)
            // {
            //     unprotectDescription(item);
            // }

            return data;
        }

        public void Post(DataEventRecord dataEventRecord)
        {
            // protectDescription(dataEventRecord);

            _context.DataEventRecords.Add(dataEventRecord);

            _context.SaveChanges();
        }

        public void Put(Guid id, DataEventRecord dataEventRecord)
        {
            // protectDescription(dataEventRecord);

            _context.DataEventRecords.Update(dataEventRecord);

            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            var entity = _context.DataEventRecords.First(t => t.Id == id);

            _context.DataEventRecords.Remove(entity);

            _context.SaveChanges();
        }

        private void protectDescription(DataEventRecord dataEventRecord)
        {
            var protectedData = _protector.Protect(dataEventRecord.Description);

            dataEventRecord.Description = protectedData;
        }

        private void unprotectDescription(DataEventRecord dataEventRecord)
        {
            var unprotectedData = _protector.Unprotect(dataEventRecord.Description);
            dataEventRecord.Description = unprotectedData;
        }
    }
}

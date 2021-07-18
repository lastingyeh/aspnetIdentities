using System;
using System.Collections.Generic;
using CustomResourceServer.Models;

namespace CustomResourceServer.Repositories
{
    public interface IDataEventRecordRepository
    {
        void Delete(Guid id);
        DataEventRecord Get(Guid id);
        List<DataEventRecord> GetAll();
        void Post(DataEventRecord dataEventRecord);
        void Put(Guid id, DataEventRecord dataEventRecord);
    }
}

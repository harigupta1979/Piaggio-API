using AppConfig;
using Core.Module.Onboard;
using MongoDB.Bson;
using MongoDB.Driver;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class MongoDBCRUD
    {
        private readonly AppConfig.IConfigManager _configuration; private string DatabaseName = "";
        public MongoDBCRUD(IConfigManager configuration)
        {
            this._configuration = configuration;
            DatabaseName = configuration.AppKey("MongodbName").ToString();
        }
        public object Query { get; private set; }

        public IMongoCollection<BsonDocument> Mongo_Connection()
        {

            MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");//127.0.0.1:27017
            //Get Database
            IMongoDatabase db = dbClient.GetDatabase(DatabaseName);
            //Get Collection/Table
            var tblDealerDocs = db.GetCollection<BsonDocument>("DealerDoc");
            return tblDealerDocs;
        }
        public IMongoCollection<BsonDocument> TDSMongo_Connection()
        {

            MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");//127.0.0.1:27017
            //Get Database
            IMongoDatabase db = dbClient.GetDatabase(DatabaseName);
            //Get Collection/Table
            var tblDealerDocs = db.GetCollection<BsonDocument>("TDSDealerDoc");
            return tblDealerDocs;
        }

        public string insert(BusinessPartnerDocumentClass obj)
        {
            var tblDealerDocs = Mongo_Connection();

            BsonDocument objInsert = new BsonDocument();
            BsonElement dealerId = new BsonElement("OnBoardId", obj.OnBoardId); //"1"
            objInsert.Add(dealerId);
            objInsert.Add(new BsonElement("DealerDocName", obj.FileName));   //"1_PANNO.pdf"
            objInsert.Add(new BsonElement("DealerDoc", obj.DocumentBas64)); //"DOCUMENT_BASE64_STRING"
            objInsert.Add(new BsonElement("FileSize", obj.FileSize));   //"1_PANNO.pdf"
            objInsert.Add(new BsonElement("DocumentType", obj.DocumentType)); //"DOCUMENT_BASE64_STRING"
            objInsert.Add(new BsonElement("FileExtension", obj.FileExtension));   //"1_PANNO.pdf"
            objInsert.Add(new BsonElement("DocumentId", obj.DocumentTypeId)); //"DOCUMENT_BASE64_STRING"
            tblDealerDocs.InsertOne(objInsert);
            var id = objInsert.ElementAt(0).Value.ToString();
            return id;
        }
        public void update(string DealerDocName, string DealerDoc)
        {
            var tblDealerDocs = Mongo_Connection();

            BsonDocument objUpdateDealerDoc = new BsonDocument();
            BsonElement dealerDocName = new BsonElement("DealerDocName", DealerDocName); //"1_PANCard.pdf"
            objUpdateDealerDoc.Add(dealerDocName);
            objUpdateDealerDoc.Add(new BsonElement("DealerDoc", DealerDoc)); //"New_DOCUMENT_BASE64_STRING"

            BsonDocument findDocByDocName = new BsonDocument(new BsonElement("DealerDocName", DealerDocName)); //"1_PANNO.pdf"
            var updateDoc = tblDealerDocs.FindOneAndReplace(findDocByDocName, objUpdateDealerDoc);
        }
        public async Task<long> delete(BusinessPartnerDocumentClass obj)
        {
            var tblDealerDocs = Mongo_Connection();  //"1_PANNO.pdf"
            //var deleteFilter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(obj.MongodbId));
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(obj.MongodbId));
            //var x = tblDealerDocs.Find();
            //var firstDocument = tblDealerDocs.Find(new BsonDocument("_id", ObjectId.Parse(obj.MongodbId)));


            //var filter2 = Builders<BsonDocument>.Filter.Eq("DealerDocName", "Dhansukhlal Prajapati_AadharCard");
            //var studentDocument = tblDealerDocs.Find(filter2);
            //var filter3 = new BsonDocument { { "_id", obj.MongodbId } };

            //var @event = await tblDealerDocs.Find(filter).SingleAsync();

            //var filter3 = new BsonDocument { { "_id", obj.MongodbId } };

            //var @event = await tblDealerDocs.Find(filter).SingleAsync();

            var @event2 = await tblDealerDocs.FindOneAndDeleteAsync(filter);
            //tblDealerDocs.Find(deleteFilter);
            return @event2 == null ? 0 : 1;
        }
        public async Task<dynamic> select(BusinessPartnerDocumentClass obj)
        {

            // BsonDocument obj =  new BsonDocument()
            var tblDealerDocs = Mongo_Connection();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(obj.MongodbId));
            //findDoc.Add(new BsonElement("DocumentType", obj.DocumentType));   //"1_PANNO.pdf"
            var @event2 = await tblDealerDocs.Find(filter).SingleAsync();
            return @event2;
        }

        public string TDSinsert(TDSDocumentClass obj)
        {
            var tblDealerDocs = TDSMongo_Connection();
            BsonDocument objInsert = new BsonDocument();
            BsonElement dealerId = new BsonElement("OnBoardId", obj.OnBoardId); //"1"
            objInsert.Add(dealerId);
            objInsert.Add(new BsonElement("TDSDocName", obj.TDSFileName));   //"1_PANNO.pdf"
            objInsert.Add(new BsonElement("TDSDoc", obj.DocumentBas64)); //"DOCUMENT_BASE64_STRING"
            objInsert.Add(new BsonElement("FileSize", obj.FileSize));   //"1_PANNO.pdf"
            objInsert.Add(new BsonElement("FileExtension", obj.FileExtension));   //"1_PANNO.pdf"
            objInsert.Add(new BsonElement("LowtdsId", obj.LowtdsId)); //"DOCUMENT_BASE64_STRING"
            tblDealerDocs.InsertOne(objInsert);
            var id = objInsert.ElementAt(0).Value.ToString();
            return id;
        }
        public async Task<dynamic> TDSselect(TDSDocumentClass obj)
        {
            var tblDealerDocs = TDSMongo_Connection();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(obj.MongodbId));
            var @event2 = await tblDealerDocs.Find(filter).SingleAsync();
            return @event2;
        }
        public async Task<long> TDSdelete(TDSDocumentClass obj)
        {
            var tblDealerDocs = TDSMongo_Connection();  //"1_PANNO.pdf"
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(obj.MongodbId));
            var @event2 = await tblDealerDocs.FindOneAndDeleteAsync(filter);
            return @event2 == null ? 0 : 1;
        }
    }
}

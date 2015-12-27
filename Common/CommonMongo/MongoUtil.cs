using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace CommonMongo
{
	public interface IDatabaseDocument { }


	// TODO : test and make more generic, remove local variables replace with method parameters
    public class MongoUtil
    {
		private string connectionString = "mongodb://dbtest-PC";
		private string databaseName = "jamestest";
		private string collectionName = "DocumentRegister";


		#region Write to Database

		// Save a DatabaseDocument to the database
		public void SaveDataToDatabase(IDatabaseDocument doc)
		{
			MongoClient client = new MongoClient(connectionString);
			MongoServer server = client.GetServer();
			MongoDatabase database = server.GetDatabase(databaseName);
			MongoCollection<IDatabaseDocument> collection = database.GetCollection<IDatabaseDocument>(collectionName);
			collection.Insert(doc);
		}

		#endregion


		#region Read from Database

		// Get databases from database
		public List<string> GetDatabasesNames()
		{
			MongoClient client = new MongoClient(connectionString);
			MongoServer server = client.GetServer();
			BsonClassMap.RegisterClassMap<IDatabaseDocument>();

			List<string> databaseNames = (List<string>) server.GetDatabaseNames();

			return databaseNames;
		}


		// TODO : fix
		// Returns a single database document
		//public IDatabaseDocument GetSingleDatabaseDocument(string documentName)
		//{
		//	MongoCollection<IDatabaseDocument> collection = GetCollectionOfDatabaseDocuments();
		//	return collection.FindOne(Query<IDatabaseDocument>.EQ(e => e.Name, documentName));
		//}


		// Get document collection from the database
		public MongoCollection<IDatabaseDocument> GetCollectionOfDatabaseDocuments()
		{
			MongoClient client = new MongoClient(connectionString);
			MongoServer server = client.GetServer();
			MongoDatabase database = server.GetDatabase(databaseName);

			return database.GetCollection<IDatabaseDocument>(collectionName);
		}


		// Get document cursor
		public List<IDatabaseDocument> GetAllDatabaseDocuments(out bool connected)
		{
			MongoClient client = new MongoClient(connectionString);
			MongoServer server = client.GetServer();

			if (server.State == MongoServerState.Disconnected || server.State == MongoServerState.Disconnecting)
			{
				connected = false;
				return null;
			}

			MongoDatabase database = server.GetDatabase(databaseName);
			MongoCollection<IDatabaseDocument> collection = database.GetCollection<IDatabaseDocument>(collectionName);
			MongoCursor<IDatabaseDocument> cursor = collection.FindAll();

			connected = true;
			return cursor.ToList();
		}

		#endregion


		#region Database Info

		// Check database connection
		private bool IsDatabaseConnected(string documentName)
		{
			MongoClient client = new MongoClient(connectionString);
			MongoServer server = client.GetServer();

			// Check server connection
			try { server.Ping(); }
			catch (SocketException)
			{
				return false;
			}

			// Check document exists
			if (server.GetDatabaseNames().Any(x => x == documentName) == false)
				return false;

			return true;
		}

		#endregion

	}
}

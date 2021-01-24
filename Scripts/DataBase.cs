using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DataBase : MonoBehaviour
{
    public SqliteConnection _dbconn;
    private string _path, _conn;



    
    private void Awake()
    {
        

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("!!!!!!!!!!!It is drod");
            _path = Get_path();
            if (!File.Exists(_path))
            {
                Debug.Log("!!!!!!!!!!!!!!try to install db");
                var loadingRequest = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, "RM_db_test.bytes"));
                loadingRequest.SendWebRequest();
                while (!loadingRequest.isDone) { }

                File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "RM_db_test.bytes"), loadingRequest.downloadHandler.data);
            }
            else if (File.Exists(_path)) Debug.Log("!!!!!!!!!!!!!! db is on droid");


        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            
            _path = Get_path();

            if (!File.Exists(_path))
            {
                _path = Application.streamingAssetsPath + "/RM_db_test.bytes";

                File.Copy(_path, Application.persistentDataPath + "/RM_db_test.bytes");

            }
            

        }

        Create_daily_bonus_table();

        


    }
    

    private string Get_path()
    {

        //_path = Application.persistentDataPath + "/driftTest.bytes";
        _path = Path.Combine(Application.persistentDataPath, "RM_db_test.bytes");
        
        return _path;


    }

    public void Set_connection()
    {

        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            
        }

    }


    public void Create_cars_table()
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();
        _query = "CREATE TABLE IF NOT EXISTS cars (carName VARCHAR UNIQUE, available INTEGER )";

        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {

            try
            {

                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();


            }
            catch (Exception e)
            {

                Debug.Log(e);

            }
        }
    }
    private void Create_daily_bonus_table()
    {
        string _query, _query2, _query3, _query4, _name;
        _name = "day_counter";
        string[] _queries = new string[4];
        SqliteCommand _cmd = new SqliteCommand();
        _queries[0] = string.Format("CREATE TABLE IF NOT EXISTS {0} (data VARCHAR UNIQUE, value INTEGER )", _name);
        _queries[1] = string.Format("insert or ignore into " + _name + " (data, value) values (\"localLastReceiveBonuseTimeKey\", '0')");
        _queries[2] = string.Format("insert or ignore into " + _name + " (data, value) values (\"localDayInRowKey\", '0')");
        _queries[3] = string.Format("insert or ignore into " + _name + " (data, value) values (\"firstTime\", '0')");
        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);

        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {

                    _cmd = _dbconn.CreateCommand();

                    _cmd.CommandText = _queries[i];
                    SqliteDataReader _reader = _cmd.ExecuteReader();


                }
                catch (Exception e)
                {

                    Debug.Log(e);

                }
            }


        }

    }
    


    public void Add_new_cars(string _table, string _carName, string _available)
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();


        _query = string.Format("insert or ignore into " + _table + " (carName, available) values (\"{0}\", {1})", _carName, _available);


        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            try
            {
                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();
                

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void Change_car_availabe(string _car, string _stat)
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();


        _query = "update cars set available = " + _stat + " where carName = \"" + _car + "\"";



        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            try
            {
                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();
                

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
    public void Create_characteristic_table(string _carName)
    {
        string _query, _query2, _query3, _query4;
        string[] _queries = new string[4];
        SqliteCommand _cmd = new SqliteCommand();
        _queries[0] = string.Format("CREATE TABLE IF NOT EXISTS {0} (characteristic VARCHAR UNIQUE, value INTEGER )", _carName);
        _queries[1] = string.Format("insert or ignore into " + _carName + " (characteristic, value) values (\"controllabilityCoeff\", '1')");
        _queries[2] = string.Format("insert or ignore into " + _carName + " (characteristic, value) values (\"motorTorqueCoeff\", '1')");
        _queries[3] = string.Format("insert or ignore into " + _carName + " (characteristic, value) values (\"FuelTankSize\", '1')");
        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);

        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            for (int i = 0; i <4; i++)
            {
                try
                {

                    _cmd = _dbconn.CreateCommand();
                    
                    _cmd.CommandText = _queries[i];
                    SqliteDataReader _reader = _cmd.ExecuteReader();


                }
                catch (Exception e)
                {

                    Debug.Log(e);

                }
            }
            
         
        }
        
    }
    public void Create_table()
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();
        _query = "CREATE TABLE IF NOT EXISTS sources (resName VARCHAR UNIQUE, amount INTEGER )";

        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {

            try
            {

                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();


            }
            catch (Exception e)
            {

                Debug.Log(e);

            }
        }

        Insert_data("resName", "amount" , "gold","0", "sources");

        Insert_data("resName","amount", "gems","0", "sources");




    }

    

    public void Read_table()
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();



        _query = "SELECT * FROM player";

        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {

            try
            {

                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();
                


            }
            catch (Exception e)
            {

                Debug.Log(e);

            }
        }
    }

    public void Update_characteristics(string _table, string _newValue, string _charType)
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();


        _query = "update " + _table + " set value = " + _newValue + " where characteristic = \"" + _charType + "\"";


        Debug.Log(_query);
        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        Debug.Log(_query);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            try
            {
                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();


            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void Update_source(string _table, string _newAmount, string _resType)
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();


        _query = "update " + _table + " set amount = " + _newAmount + " where resName = \"" + _resType + "\"";



        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            try
            {
                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();
                

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void Update_car_bought(string _table, string _newAmount, string _carName)
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();


        _query = "update " + _table + " set bought = " + _newAmount + " where carName = \"" + _carName + "\"";



        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            try
            {
                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();


            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }



    public void Insert_data(string _dataType1, string _dataType2, string _data1, string _data2, string _table)
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();


        _query = string.Format("insert or ignore into " + _table + " (" + _dataType1+", " +_dataType2  + ") values (\"{0}\", {1})", _data1, _data2);

        _path = Get_path();

        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            try
            {
                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();
                

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

    }

    public string Get_res_inf(string _table, string _resName)
    {
        

        string _data;
        _path = Get_path();
        _conn = "URI=file:" + _path;

        using (_dbconn = new SqliteConnection(_conn))
        {



            _dbconn.Open(); //Open connection to the database.
            IDbCommand _dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT amount FROM " + _table + " where resName ='" + _resName + "'";// table name


           

            _dbcmd.CommandText = sqlQuery;
            IDataReader reader = _dbcmd.ExecuteReader();
            while (reader.Read())
            {
                int _goldAmount;


                _goldAmount = reader.GetInt32(0);
                _data = _goldAmount.ToString();

                return _data;

            }
            reader.Close();
            reader = null;
            _dbcmd.Dispose();
            _dbcmd = null;
            _dbconn.Close();

            _data = "Error";
            return _data;
        }

    }

    public string Get_data(string _table, string _gettingDataType, string _refPoint, string _refPointValue)
    {

        _path = Get_path();

        _conn = "URI=file:" + _path;

        using (_dbconn = new SqliteConnection(_conn))
        {



            _dbconn.Open(); //Open connection to the database.
            IDbCommand _dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT " + _gettingDataType + " FROM " + _table + " where " + _refPoint + " =" + _refPointValue + "";
            

            _dbcmd.CommandText = sqlQuery;
            IDataReader reader = _dbcmd.ExecuteReader();
            string _data;

            while (reader.Read())
            {


                _data = reader.GetString(0);


                return (_data);

            }
            reader.Close();
            reader = null;
            _dbcmd.Dispose();
            _dbcmd = null;
            _dbconn.Close();


            _data = "Error";
            return _data;
        }
    }


    public string Get_data_int(string _table, string _gettingDataType, string _refPoint, string _refPointValue)
    {

        _path = Get_path();

        _conn = "URI=file:" + _path;

        using (_dbconn = new SqliteConnection(_conn))
        {



            _dbconn.Open(); //Open connection to the database.
            IDbCommand _dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT " + _gettingDataType + " FROM " + _table + " where " + _refPoint + " =" + _refPointValue + "";
            sqlQuery = sqlQuery.Replace("(Clone)", "");
            

            _dbcmd.CommandText = sqlQuery;
            IDataReader reader = _dbcmd.ExecuteReader();
            string _data;
            int _intData;
            while (reader.Read())
            {
                _intData = reader.GetInt32(0);

                _data = _intData.ToString();
                


                return (_data);

            }
            reader.Close();
            reader = null;
            _dbcmd.Dispose();
            _dbcmd = null;
            _dbconn.Close();


            _data = "Error";
            return _data;
        }

    }

    public void Upload_day_counter(string _dataType, int _data)
    {
        string _query;
        SqliteCommand _cmd = new SqliteCommand();


        _query = "update day_counter set value = " + _data.ToString() + " where data = \"" + _dataType+ "\"";



        _path = Get_path();
        _dbconn = new SqliteConnection("URI=file:" + _path);
        _dbconn.Open();

        if (_dbconn.State == ConnectionState.Open)
        {
            try
            {
                _cmd = _dbconn.CreateCommand();
                _cmd.CommandText = _query;
                SqliteDataReader _reader = _cmd.ExecuteReader();


            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public string Get_data_day_counter(string _dataName)
    {
        _path = Get_path();

        _conn = "URI=file:" + _path;

        using (_dbconn = new SqliteConnection(_conn))
        {



            _dbconn.Open(); //Open connection to the database.
            IDbCommand _dbcmd = _dbconn.CreateCommand();
            string sqlQuery = "SELECT value FROM day_counter where data = \"" + _dataName + "\"";
            sqlQuery = sqlQuery.Replace("(Clone)", "");


            _dbcmd.CommandText = sqlQuery;
            IDataReader reader = _dbcmd.ExecuteReader();
            string _data;
            int _intData;
            while (reader.Read())
            {
                _intData = reader.GetInt32(0);

                _data = _intData.ToString();



                return (_data);

            }
            reader.Close();
            reader = null;
            _dbcmd.Dispose();
            _dbcmd = null;
            _dbconn.Close();


            _data = "Error";
            return _data;
        }
    }
}
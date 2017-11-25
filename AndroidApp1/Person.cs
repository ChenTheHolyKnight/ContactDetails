
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using System;

[Serializable]
public class Person
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public override string ToString()
    {
        return string.Format("[Person: Id={0},FirstName={1},LastName={2},PhoneNumber={3}]", Id, FirstName, LastName, PhoneNumber);
    }
}

public class DataBase
{
    private string folder;
    private string dataPath;
    private static DataBase dataBase;

    private DataBase(string folder)
    {
        this.folder = folder;
        dataPath = System.IO.Path.Combine(folder, "Person.db");
    }

    public static DataBase getInstance(string folder)
    {
        if (dataBase == null)
        {
            dataBase = new DataBase(folder);
        }

        return dataBase;
    }
   
    public bool CreateDataBase()
    {
        try
        {
            using (var connection = new SQLiteConnection(dataPath))
            {
                connection.CreateTable<Person>();
                return true;
            }
        }
        catch (SQLiteException e)
        {
            Log.Info("SQLiteEX", e.Message);
            return false;
        }
    }

    public bool insertIntoTablePerson(Person person)
    {
        try
        {
            using (var connection = new SQLiteConnection(dataPath))
            {
                connection.Insert(person);
                return false;
            }
        }
        catch (SQLiteException e)
        {
            Log.Info("SQLiteEX", e.Message);
            return false;
        }
    }

    public List<Person> selectTablePerson()
    {
        try
        {
            using (var connection = new SQLiteConnection(dataPath))
            {
                return connection.Table<Person>().ToList();
            }
        }
        catch (SQLiteException e)
        {
            Log.Info("SQLiteEX", e.Message);
            return null;
        }
    }

    public bool updateTablePerson(Person person)
    {
        try
        {
            using (var connection = new SQLiteConnection(dataPath))
            {
                connection.Query<Person>("Update Person set FirstName=?,LastName=?,PhoneNumber=? Where Id=?", person.FirstName, person.LastName, person.PhoneNumber, person.Id);
                return true;
            }
        }
        catch (SQLiteException e)
        {
            Log.Info("SQLiteEX", e.Message);
            return false;
        }
    }

    public bool deleteTablePerson(Person person)
    {
        try
        {
            using (var connection = new SQLiteConnection(dataPath))
            {
                connection.Delete(person);
                return true;
            }
        }
        catch (SQLiteException e)
        {
            Log.Info("SQLiteEX", e.Message);
            return false;
        }

    }
}
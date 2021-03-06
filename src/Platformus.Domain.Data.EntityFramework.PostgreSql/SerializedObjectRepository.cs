﻿// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Platformus.Domain.Data.Abstractions;
using Platformus.Domain.Data.Entities;

namespace Platformus.Domain.Data.EntityFramework.PostgreSql
{
  public class SerializedObjectRepository : RepositoryBase<SerializedObject>, ISerializedObjectRepository
  {
    public SerializedObject WithKey(int cultureId, int objectId)
    {
      return this.dbSet.FirstOrDefault(so => so.CultureId == cultureId && so.ObjectId == objectId);
    }

    public SerializedObject WithCultureIdAndUrlPropertyStringValue(int cultureId, string urlPropertyStringValue)
    {
      return this.dbSet.FirstOrDefault(so => so.CultureId == cultureId && string.Equals(so.UrlPropertyStringValue, urlPropertyStringValue, System.StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<SerializedObject> FilteredByCultureIdAndClassId(int cultureId, int classId)
    {
      return this.dbSet.Where(so => so.CultureId == cultureId && so.ClassId == classId);
    }

    public IEnumerable<SerializedObject> FilteredByCultureIdAndClassId(int cultureId, int classId, Params @params)
    {
      return this.dbSet.FromSql(
        this.GetSortedSelectQuerySql("\"SerializedObjects\".\"ClassId\" = {1}", @params),
        cultureId, classId
      );
    }

    public IEnumerable<SerializedObject> FilteredByCultureIdAndClassIdAndObjectId(int cultureId, int classId, int objectId, Params @params)
    {
      return this.dbSet.FromSql(
        this.GetSortedSelectQuerySql("\"SerializedObjects\".\"ClassId\" = {1} AND \"SerializedObjects\".\"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"ForeignId\" = {2})", @params),
        cultureId, classId, objectId
      );
    }

    public IEnumerable<SerializedObject> Primary(int cultureId, int objectId)
    {
      return this.dbSet.FromSql(
        this.GetUnsortedSelectQuerySql("\"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"ForeignId\" = {1})"),
        cultureId, objectId
      );
    }

    public IEnumerable<SerializedObject> Primary(int cultureId, int objectId, Params @params)
    {
      return this.dbSet.FromSql(
        this.GetSortedSelectQuerySql("\"SerializedObjects\".\"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"ForeignId\" = {1})", @params),
        cultureId, objectId
      );
    }

    public IEnumerable<SerializedObject> Primary(int cultureId, int memberId, int objectId)
    {
      return this.dbSet.FromSql(
        this.GetUnsortedSelectQuerySql("\"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"MemberId\" = {1} AND \"ForeignId\" = {2})"),
        cultureId, memberId, objectId
      );
    }

    public IEnumerable<SerializedObject> Primary(int cultureId, int memberId, int objectId, Params @params)
    {
      return this.dbSet.FromSql(
        this.GetSortedSelectQuerySql("\"SerializedObjects\".\"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"MemberId\" = {1} AND \"ForeignId\" = {2})", @params),
        cultureId, memberId, objectId
      );
    }

    public IEnumerable<SerializedObject> Foreign(int cultureId, int objectId)
    {
      return this.dbSet.FromSql(
        this.GetUnsortedSelectQuerySql("\"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"ForeignId\" = {1})"),
        cultureId, objectId
      );
    }

    public IEnumerable<SerializedObject> Foreign(int cultureId, int objectId, Params @params)
    {
      return this.dbSet.FromSql(
        this.GetSortedSelectQuerySql("\"SerializedObjects\".\"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"ForeignId\" = {1})", @params),
        cultureId, objectId
      );
    }

    public IEnumerable<SerializedObject> Foreign(int cultureId, int memberId, int objectId)
    {
      return this.dbSet.FromSql(
        this.GetUnsortedSelectQuerySql("\"ObjectId\" IN (SELECT \"ForeignId\" FROM \"Relations\" WHERE \"MemberId\" = {1} AND \"PrimaryId\" = {2})"),
        cultureId, memberId, objectId
      );
    }

    public IEnumerable<SerializedObject> Foreign(int cultureId, int memberId, int objectId, Params @params)
    {
      return this.dbSet.FromSql(
        this.GetSortedSelectQuerySql("\"SerializedObjects\".\"ObjectId\" IN (SELECT \"ForeignId\" FROM \"Relations\" WHERE \"MemberId\" = {1} AND \"PrimaryId\" = {2})", @params),
        cultureId, memberId, objectId
      );
    }

    public void Create(SerializedObject serializedObject)
    {
      this.dbSet.Add(serializedObject);
    }

    public void Edit(SerializedObject serializedObject)
    {
      this.storageContext.Entry(serializedObject).State = EntityState.Modified;
    }

    public void Delete(int cultureId, int objectId)
    {
      this.Delete(this.WithKey(cultureId, objectId));
    }

    public void Delete(SerializedObject serializedObject)
    {
      this.dbSet.Remove(serializedObject);
    }

    public int CountByCultureIdAndClassId(int cultureId, int classId, Params @params)
    {
      if (@params == null || @params.Filtering == null || string.IsNullOrEmpty(@params.Filtering.Query))
        return this.dbSet.Count(so => so.CultureId == cultureId && so.ClassId == classId);

      int result = 0;
      NpgsqlConnection connection = (this.storageContext as DbContext).Database.GetDbConnection() as NpgsqlConnection;

      try
      {
        connection.Open();

        using (NpgsqlCommand command = connection.CreateCommand())
        {
          command.CommandText = "SELECT COUNT(*) FROM \"SerializedObjects\" WHERE \"CultureId\" = @CultureId AND \"ClassId\" = @ClassId AND \"ObjectId\" IN (SELECT \"ObjectId\" FROM \"Properties\" WHERE \"StringValueId\" IN (SELECT \"DictionaryId\" FROM \"Localizations\" WHERE \"Value\" LIKE @Query))";
          command.Parameters.AddWithValue("@CultureId", cultureId);
          command.Parameters.AddWithValue("@ClassId", classId);
          command.Parameters.AddWithValue("@Query", "%" + @params.Filtering.Query + "%");

          using (DbDataReader dataReader = command.ExecuteReader())
            if (dataReader.HasRows)
              while (dataReader.Read())
                result = dataReader.GetInt32(0);
        }
      }

      catch (System.Exception e) { connection.Close(); }

      return result;
    }

    public int CountByCultureIdAndClassIdAndObjectId(int cultureId, int classId, int objectId, Params @params)
    {
      int result = 0;
      NpgsqlConnection connection = (this.storageContext as DbContext).Database.GetDbConnection() as NpgsqlConnection;

      try
      {
        connection.Open();

        using (NpgsqlCommand command = connection.CreateCommand())
        {
          if (@params == null || @params.Filtering == null || string.IsNullOrEmpty(@params.Filtering.Query))
          {
            command.CommandText = "SELECT COUNT(*) FROM \"SerializedObjects\" WHERE \"CultureId\" = @CultureId AND \"ClassId\" = @ClassId AND \"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"ForeignId\" = @ObjectId)";
            command.Parameters.AddWithValue("@CultureId", cultureId);
            command.Parameters.AddWithValue("@ClassId", classId);
            command.Parameters.AddWithValue("@ObjectId", objectId);
          }

          else
          {
            command.CommandText = "SELECT COUNT(*) FROM \"SerializedObjects\" WHERE \"CultureId\" = @CultureId AND \"ClassId\" = @ClassId AND \"ObjectId\" IN (SELECT \"PrimaryId\" FROM \"Relations\" WHERE \"ForeignId\" = @ObjectId) AND \"ObjectId\" IN (SELECT \"ObjectId\" FROM \"Properties\" WHERE \"StringValueId\" IN (SELECT \"DictionaryId\" FROM \"Localizations\" WHERE \"Value\" LIKE @Query))";
            command.Parameters.AddWithValue("@CultureId", cultureId);
            command.Parameters.AddWithValue("@ClassId", classId);
            command.Parameters.AddWithValue("@ObjectId", objectId);
            command.Parameters.AddWithValue("@Query", "%" + @params.Filtering.Query + "%");
          }

          using (DbDataReader dataReader = command.ExecuteReader())
            if (dataReader.HasRows)
              while (dataReader.Read())
                result = dataReader.GetInt32(0);
        }
      }

      catch { connection.Close(); }

      return result;
    }

    private string GetUnsortedSelectQuerySql(string additionalWhereClause)
    {
      StringBuilder sql = new StringBuilder("SELECT * FROM \"SerializedObjects\" WHERE \"CultureId\" = {0} AND ");

      sql.Append(additionalWhereClause);
      return sql.ToString();
    }

    private string GetSortedSelectQuerySql(string additionalWhereClause, Params @params)
    {
      if (@params.Sorting == null)
        return this.GetUnsortedSelectQuerySql(additionalWhereClause);

      StringBuilder sql = new StringBuilder(
        this.GetSortedBaseSelectQuerySql(additionalWhereClause, @params.Sorting.StorageDataType == StorageDataType.String)
      );

      if (@params.Sorting.StorageDataType == StorageDataType.Integer)
        sql.Append(this.GetSortedByIntegerValueSelectQuerySql(@params));

      if (@params.Sorting.StorageDataType == StorageDataType.Decimal)
        sql.Append(this.GetSortedByDecimalValueSelectQuerySql(@params));

      if (@params.Sorting.StorageDataType == StorageDataType.String)
        sql.Append(this.GetSortedByStringValueSelectQuerySql(@params));

      if (@params.Sorting.StorageDataType == StorageDataType.DateTime)
        sql.Append(this.GetSortedByDateTimeValueSelectQuerySql(@params));

      if (@params.Paging != null)
        sql.Append($" LIMIT {@params.Paging.Take} OFFSET {@params.Paging.Skip}");

      return sql.ToString();
    }

    private string GetSortedBaseSelectQuerySql(string additionalWhereClause, bool joinLocalizations)
    {
      return
        "SELECT \"SerializedObjects\".\"CultureId\", \"SerializedObjects\".\"ObjectId\", \"SerializedObjects\".\"ClassId\", \"SerializedObjects\".\"UrlPropertyStringValue\", \"SerializedObjects\".\"SerializedProperties\" FROM \"SerializedObjects\" " +
        "INNER JOIN \"Classes\" ON \"Classes\".\"Id\" = \"SerializedObjects\".\"ClassId\" " +
        "INNER JOIN \"Members\" ON \"Members\".\"ClassId\" = \"SerializedObjects\".\"ClassId\" OR \"Members\".\"ClassId\" = \"Classes\".\"ClassId\" " +
        "INNER JOIN \"Properties\" ON \"Properties\".\"ObjectId\" = \"SerializedObjects\".\"ObjectId\" AND \"Properties\".\"MemberId\" = \"Members\".\"Id\" " +
        (joinLocalizations ? "INNER JOIN \"Localizations\" ON \"Localizations\".\"DictionaryId\" = \"Properties\".\"StringValueId\" " : null) +
        "WHERE \"SerializedObjects\".\"CultureId\" = {0} AND " + additionalWhereClause + " AND ";
    }

    private string GetSortedByIntegerValueSelectQuerySql(Params @params)
    {
      return "\"Members\".\"Id\" = " + @params.Sorting.MemberId + " ORDER BY \"Properties\".\"IntegerValue\" " + @params.Sorting.Direction;
    }

    private string GetSortedByDecimalValueSelectQuerySql(Params @params)
    {
      return "\"Members\".\"Id\" = " + @params.Sorting.MemberId + " ORDER BY \"Properties\".\"DecimalValue\" " + @params.Sorting.Direction;
    }

    private string GetSortedByStringValueSelectQuerySql(Params @params)
    {
      string filteringSql = null;

      if (@params.Filtering != null)
        filteringSql = " AND \"SerializedObjects\".\"ObjectId\" IN (SELECT \"ObjectId\" FROM \"Properties\" WHERE \"StringValueId\" IN (SELECT \"DictionaryId\" FROM \"Localizations\" WHERE \"Value\" LIKE '%" + @params.Filtering.Query + "%'))";

      return "\"Members\".\"Id\" = " + @params.Sorting.MemberId + " AND (\"Localizations\".\"CultureId\" = 1 OR \"Localizations\".\"CultureId\" = {0})" + filteringSql + " ORDER BY \"Localizations\".\"Value\" " + @params.Sorting.Direction;
    }

    private string GetSortedByDateTimeValueSelectQuerySql(Params @params)
    {
      return "\"Members\".\"Id\" = " + @params.Sorting.MemberId + " ORDER BY \"Properties\".\"DateTimeValue\" " + @params.Sorting.Direction;
    }
  }
}
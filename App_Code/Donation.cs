﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient; //classic ADO
using System.Configuration;

/// <summary>
/// Summary description for Donation
/// </summary>
public class Donation
{
    SqlConnection connect;
    //constructor
	public Donation()
	{
        connect = new SqlConnection(ConfigurationManager.ConnectionStrings["CommunityAssistConnectionString"].ConnectionString);
	}

    public DataTable GetDonors()
    {
        string sql = "select distinct p.PersonKey, PersonLastName " +
            "From Person p " +
            "inner join Donation d " +
            "on p.PersonKey=d.PersonKey " +
            "Order by PersonLastName";
        SqlCommand cmd = new SqlCommand(sql, connect);
        SqlDataReader reader = null;
        DataTable table = new DataTable();
        connect.Open();
        reader = cmd.ExecuteReader();
        table.Load(reader);
        reader.Dispose();
        connect.Close(); //open as little as possible
        return table;

    }

    public void WriteDonation(int pk, decimal amount)
    {
        string sql = "Insert into Donation (DonationDate, DonationAmount, PersonKey) Values (@Date, @amount, @person)";
        SqlCommand cmd = new SqlCommand(sql, connect);
        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
        cmd.Parameters.AddWithValue("@amount", amount);
        cmd.Parameters.AddWithValue("@person", pk);

        connect.Open();
        cmd.ExecuteNonQuery();
        connect.Close();
    }

    public DataTable GetDonationTotals()
    {
        string sql = "Select Sum(DonationAmount) From Donation Group by Year(DonationDate), Month(DonationDate)";
        SqlCommand cmd = new SqlCommand(sql, connect);
        SqlDataReader reader = null;
        DataTable table = new DataTable();
        connect.Open();
        reader = cmd.ExecuteReader();
        table.Load(reader);
        reader.Dispose();
        connect.Close();

        return table;
    }
}
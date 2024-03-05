using System;
using System.ComponentModel.DataAnnotations;

namespace Ledger.Ledger.Web.Models
{
    public class Stock // Stock is the main subject of the trading operations, it refers to the stock of a company and so its StockId, StockName fields are unique.
                       // It also has OpenDate, InitialStock, InitialPrice, CurrentStock, and CurrentPrice fields. InitialStock and InitialPrice are determined when
                       // the stock opens to the market and CurrentStock and CurrentPrice values are updated according to the transactions (buy/sell) operations in the market.

    {
    [Key] public int StockId { get; set; } // randomly generated integer value
    public string StockName { get; set; }
    public DateTime OpenDate { get; set; } = DateTime.Now.ToUniversalTime();
    public int InitialStock { get; set; }
    public double InitialPrice { get; set; }
    public int CurrentStock { get; set; }
    public double CurrentPrice { get; set; }
    public double HighestPrice { get; set; }
    public double LowestPrice { get; set; }
    public bool Status { get; set; } = true; //true:active, false:deactive
    

    public Stock()
    {

    }

    public Stock(int stockId, string stockName, int initialStock, double initialPrice, int currentStock, double currentPrice, double highestPrice, double lowestPrice, bool status)
    {
        StockId = stockId;
        StockName = stockName;
        InitialStock = initialStock;
        InitialPrice = initialPrice;
        CurrentStock = currentStock;
        CurrentPrice = currentPrice;
        HighestPrice = highestPrice;
        LowestPrice = lowestPrice;
        Status = status;
    }

    public override string ToString()
    {
        return $"StockId: {StockId}, StockName: {StockName}, OpenDate: {OpenDate}, " +
               $"InitialStock: {InitialStock}, InitialPrice: {InitialPrice}, " +
               $"CurrentStock: {CurrentStock}, CurrentPrice: {CurrentPrice}, " +
               $"HighestPrice: {HighestPrice}, LowestPrice: {LowestPrice}, Status: {Status}";
    }
    
    }
}
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ProTemplate.Utility
{
    public class Constants
    {
        public const int CommonGridViewPageSize = 100;

        public static bool IsNumeric(string strText)
        {
            try
            {
                int.Parse(strText);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public static bool IsDouble(string strText)
        {
            try
            {
                double.Parse(strText);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string PrintSQL = @"WITH    OK
          AS ( SELECT   A.DeclarationId ,
                        B.SortOrder ,
                        NULL AS Number ,
                        NULL AS SubNumber ,
                        NULL AS GoodsName ,
                        NULL AS Model ,
                        NULL AS DeclaredQuantity ,
                        NULL AS DeclaredUnitCode ,
                        NULL AS LegalQuantity ,
                        NULL AS LegalUnitCode ,
                        NULL AS SecondQuantity ,
                        NULL AS SecondUnitCode ,
                        NULL AS CountryCode ,
                        NULL AS DeclaredPrice ,
                        NULL AS DeclaredTotalPrice ,
                        NULL AS CurrencyCode ,
                        NULL AS DutyCode, 
                        NULL AS ControlItem
               FROM     ( SELECT    DeclarationID
                          FROM      [DeclarationItem]
                          GROUP BY  DeclarationID
                        ) AS A
                        CROSS JOIN [CrossJoinTemp] AS B
               UNION ALL
               SELECT   DeclarationID ,
                        SortOrder ,
                        Number ,
                        SubNumber ,
                        [Name] AS GoodsName ,
                        Model ,
                        DeclaredQuantity ,
                        DeclaredUnitCode ,
                        LegalQuantity ,
                        LegalUnitCode ,
                        SecondQuantity ,
                        SecondUnitCode ,
                        CountryCode ,
                        DeclaredPrice ,
                        DeclaredTotalPrice ,
                        CurrencyCode ,
                        DutyCode,
                        ControlItem
               FROM     [DeclarationItem]
             )
    SELECT  Detail.DeclarationID ,
            Detail.SortOrder ,
            Detail.Number ,
            Detail.SubNumber ,
            Detail.GoodsName ,
            Detail.Model ,
            Detail.DeclaredQuantity ,
            Detail.DeclaredUnitCode ,
            Detail.LegalQuantity ,
            Detail.LegalUnitCode ,
            Detail.SecondQuantity ,
            Detail.SecondUnitCode ,
            Detail.CountryCode AS DetailCountryCode ,
            Detail.DeclaredPrice ,
            Detail.DeclaredTotalPrice ,
            Detail.CurrencyCode ,
            Detail.ControlItem,
            Main.* ,
            Customhouse.Name AS CustomhouseName ,
            Transport.Name AS TransportName ,
            Trade.Name AS TradeName ,
            Levy.Name AS LevyName ,
            Pay.Name AS PayName ,
            Country.Name AS CountryName ,
            Port.Name AS PortName ,
            District.Name AS DistrictName ,
            TransactionT.Name AS TransactionName ,
            Wrap.Name AS WrapName ,
            DeclaredUnit.Name AS DeclaredUnitName ,
            LegalUnit.Name AS LegalUnitName ,
            SecondUnit.Name AS SecondUnitName ,
            DetailCountry.Name AS DetailCountryName ,
            Currency.Name AS DetailCurrencyName ,
            Currency.Symbol AS DetailCurrencySymbol ,
            Duty.Name AS DutyName,
            [dbo].[GetDeclarationDocumentCodes] (Main.ID) as DocumentCodes,
            [dbo].[GetDeclarationContainerNumbers] (Main.ID) as ContainerNumbers
    FROM    ( SELECT    DeclarationID ,
                        SortOrder ,
                        Number ,
                        SubNumber ,
                        GoodsName ,
                        Model ,
                        DeclaredQuantity ,
                        DeclaredUnitCode ,
                        LegalQuantity ,
                        LegalUnitCode ,
                        SecondQuantity ,
                        SecondUnitCode ,
                        CountryCode ,
                        DeclaredPrice ,
                        DeclaredTotalPrice ,
                        CurrencyCode ,
                        DutyCode ,
                        ControlItem,
                        ROW_NUMBER() OVER ( PARTITION BY DeclarationID ORDER BY SortOrder ) AS RowNumber ,
                        ( SELECT    CASE WHEN COUNT(*) > 0
                                              AND COUNT(*) % 5 > 0
                                         THEN COUNT(*) - COUNT(*) % 5
                                         WHEN COUNT(*) > 0
                                              AND COUNT(*) % 5 = 0
                                         THEN COUNT(*) - 5
                                         ELSE 5
                                    END
                          FROM      OK
                          WHERE     ( DeclarationID = A.DeclarationID )
                        ) AS CountRows
              FROM      OK AS A
            ) AS Detail
            INNER JOIN [Declaration] AS Main ON Detail.DeclarationID = Main.ID
            LEFT OUTER JOIN [Customhouse] AS Customhouse ON Main.CustomhouseCode = Customhouse.Code
            LEFT OUTER JOIN [Transport] AS Transport ON Main.TransportCode = Transport.Code
            LEFT OUTER JOIN [Trade] AS Trade ON Main.TradeCode = Trade.Code
            LEFT OUTER JOIN [Levy] AS Levy ON Main.LevyCode = Levy.Code
            LEFT OUTER JOIN [Pay] AS Pay ON Main.PayCode = Pay.Code
            LEFT OUTER JOIN [Country] AS Country ON Main.CountryCode = Country.Code
            LEFT OUTER JOIN [Port] AS Port ON Main.PortCode = Port.Code
            LEFT OUTER JOIN [District] AS District ON Main.DistrictCode = District.Code
            LEFT OUTER JOIN [Transaction] AS TransactionT ON Main.[TransactionCode] = TransactionT.Code
            LEFT OUTER JOIN [Wrap] AS Wrap ON Main.WrapCode = Wrap.Code
            LEFT OUTER JOIN [Unit] AS DeclaredUnit ON Detail.DeclaredUnitCode = DeclaredUnit.Code
            LEFT OUTER JOIN [Unit] AS LegalUnit ON Detail.LegalUnitCode = LegalUnit.Code
            LEFT OUTER JOIN [Unit] AS SecondUnit ON Detail.SecondUnitCode = SecondUnit.Code
            LEFT OUTER JOIN [Country] AS DetailCountry ON Detail.CountryCode = DetailCountry.Code
            LEFT OUTER JOIN [Currency] AS Currency ON Detail.CurrencyCode = Currency.Code
            LEFT OUTER JOIN [Duty] AS Duty ON Detail.DutyCode = Duty.Code
    WHERE   RowNumber <= CountRows
            AND main.ID in ({0}) and Main.DeclarationStatus in ('报关完成','查验','关封','注销','退单')";
    }
}

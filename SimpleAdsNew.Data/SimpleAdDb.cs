using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SimpleAdsNew.Data
{
    public class SimpleAdDb
    {
        private string _connectionString;

        public SimpleAdDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddSimpleAd(SimpleAd ad)
        {
            ad.Date = DateTime.Now;
            using (var context = new SimpleAdsContext(_connectionString))
            {
                context.Ads.Add(ad);
                context.SaveChanges();
            }
        }

        public IEnumerable<SimpleAd> GetAds()
        {
            using (var context = new SimpleAdsContext(_connectionString))
            {
                return context.Ads.Include(a => a.User)
                    .OrderByDescending(a => a.Date).ToList();
            }
        }

        public IEnumerable<SimpleAd> GetAdsForUser(int userId)
        {
            using (var context = new SimpleAdsContext(_connectionString))
            {
                return context.Ads
                    .Include(a => a.User)
                    .Where(a => a.UserId == userId).ToList();
            }
        }

        public int GetUserIdForAd(int adId)
        {
            using (var context = new SimpleAdsContext(_connectionString))
            {
                return context.Ads.FirstOrDefault(a => a.Id == adId).UserId;
            }
        }

        public void Delete(int id)
        {
            using (var context = new SimpleAdsContext(_connectionString))
            {
                context.Database.ExecuteSqlCommand("DELETE FROM Ads WHERE Id = @id",
                    new SqlParameter("@id", id));
            }
        }
    }
}
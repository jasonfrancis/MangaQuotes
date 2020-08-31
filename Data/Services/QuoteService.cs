using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace mangaQuotes.Data.Services
{
    public class QuoteService
    {
        #region Private members
        private QuoteDbContext dbContext;
        #endregion

        #region Constructor
        public QuoteService(QuoteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// This method returns the list of quotes
        /// </summary>
        /// <returns></returns>
        public async Task<List<Quote>> GetQuotesAsync()
        {
            return await GetAllAndRelated().ToListAsync();
        }

        private async Task<Quote> SetRelatedProperties (Quote quote)
        {
            quote.Character = await dbContext.Characters.SingleAsync(c => c.Id.Equals(quote.Character.Id));
            quote.ParentQuote = quote.ParentQuote == null ? null : await dbContext.Quotes.SingleOrDefaultAsync(q => q.Id.Equals(quote.ParentQuote.Id));

            return quote;
        }

        /// <summary>
        /// This method add a new quote to the DbContext and saves it
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public async Task<Quote> AddQuoteAsync(Quote quote)
        {
            var validationContext = new ValidationContext(quote);
            Validator.ValidateObject(quote, validationContext, true);
            try
            {
                quote = await SetRelatedProperties(quote);
                dbContext.Quotes.Add(quote);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return quote;
        }

        /// <summary>
        /// This method update and existing quote and saves the changes
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public async Task<Quote> UpdateQuoteAsync(Quote quote)
        {
            var validationContext = new ValidationContext(quote);
            Validator.ValidateObject(quote, validationContext, true);
            try
            {
                quote = await SetRelatedProperties(quote);
                var quoteExist = GetAllAndRelated().FirstOrDefault(p => p.Id == quote.Id);
                if (quoteExist != null)
                {
                    dbContext.Update(quote);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return quote;
        }

        /// <summary>
        /// This method removes and existing quote from the DbContext and saves it
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public async Task DeleteQuoteAsync(Quote quote)
        {
            try
            {
                dbContext.Quotes.Remove(quote);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public IQueryable<Quote> GetAllAndRelated()
        {
            return GetAllAndRelated(dbContext);
        }

        public static IQueryable<Quote> GetAllAndRelated(QuoteDbContext context)
        {
            return context.Quotes
            .Include(q => q.ParentQuote)
            .Include(q => q.Character);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using PracujPl.Commons.Database;
using PracujPl.Commons.RegionOffer;
using Timer = System.Timers.Timer;



namespace PracujPlRegionOffers
{
    class Program
    {
        // Initialization
        //  - Simple trace source, production like solution would need self cleaning/archiving logger.
        static TraceSource Log = new TraceSource("PracujPlJobOffers");
        static int RetryCount;
        static int RetryIntervalSeconds;
        static int RepeatIntervalSeconds;
        static Timer Timer;
        

        static void Main(string[] args)
        {
            Log.TraceEvent(TraceEventType.Information, 0,"Started");
            try
            {
                Console.WriteLine("# ");
                Console.WriteLine("#   Press \'q\' anytime to quit");
                Console.WriteLine("# ");
                // Initialize config values
                if (!int.TryParse(ConfigurationManager.AppSettings["RetryCount"], out RetryCount))
                    throw new ConfigurationErrorsException("Invalid RetryCount value");
                Log.TraceEvent(TraceEventType.Information, 0, "Retry count: {0}", RetryCount);

                if (!int.TryParse(ConfigurationManager.AppSettings["RetryIntervalSeconds"], out RetryIntervalSeconds))
                    throw new ConfigurationErrorsException("Invalid RetryIntervalSeconds value");
                Log.TraceEvent(TraceEventType.Information, 0, "Retry interval seconds: {0}", RetryIntervalSeconds);

                if (!int.TryParse(ConfigurationManager.AppSettings["RepeatIntervalSeconds"], out RepeatIntervalSeconds))
                    throw new ConfigurationErrorsException("Invalid RepeatIntervalSeconds value");
                Log.TraceEvent(TraceEventType.Information, 0, "Repeat interval (seconds): {0}", RepeatIntervalSeconds);

                // Initialize Timer to run loading of region job offers every given time
                Timer = new Timer(TimeSpan.FromSeconds(RepeatIntervalSeconds).TotalMilliseconds);
                Timer.Elapsed += LoadNewDataIntoDb;
                Timer.AutoReset = false;
                Timer.Enabled = true;
                // Keeps timer alive
                GC.KeepAlive(Timer);

                // Run in loop until instructed to stop
                
                while (Console.ReadKey().KeyChar != 'q') { }

                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Log.TraceEvent(TraceEventType.Error, 0, "Got exception");
                Log.TraceData(TraceEventType.Error, 0, e);
                Environment.Exit(1);
            }

        }

        static void LoadNewDataIntoDb(object sender, ElapsedEventArgs a)
        {
            try
            {
                // Prepare 
                ILoadRegionJobOffers loadRegionJobOffers = new LoadRegionJobOffersFromSite();

                IEnumerable<RegionJobOffer> regions = null;

                // Load data from site, try again if fail for RetryCount times
                for (var retry = 0; retry < RetryCount; retry++)
                {
                    try
                    {
                        // Try load
                        regions = loadRegionJobOffers.Get();

                        // Break loop when data loaded
                        if (regions != null)
                        {
                            Log.TraceEvent(TraceEventType.Verbose, 0,"Retrived {0} records from site", regions.Count());
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        // Log exceptions and try again
                        Log.TraceEvent(TraceEventType.Error, 0, "Error loading data from site. Retry count left: {0}", RetryCount - retry);
                        Log.TraceData(TraceEventType.Error, 0, e);
                        Thread.Sleep(RetryIntervalSeconds);
                    }
                }

                using (var ctx = new RegionJobOffersContext())
                {
                    // Attach Regions to Regions table to avoid recreation of foreign key
                    var regionsfk = regions.Select(r => new RegionJobOffer
                    {
                        LoadDateTime = r.LoadDateTime,
                        RegionId = ctx.Regions.SingleOrDefault( w => w.RegionName == r.RegionId.RegionName),
                        JobOffers = r.JobOffers
                    });

                    //Save retrived rows
                    ctx.RegionJobOffers.AddRange(regionsfk);
                    ctx.SaveChanges();

                }
                Timer.Start();
            }
            catch (Exception e)
            {
                Log.TraceEvent(TraceEventType.Error, 0, "Got exception");
                Log.TraceData(TraceEventType.Error, 0, e);
                Environment.Exit(1);
            }
        }
    }
}

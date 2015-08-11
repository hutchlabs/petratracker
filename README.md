# petratracker
A payments and schedules workflow tracker

This is a new comment
#Changes to the payments table
Schedules might not function properly if there are any references to payment deal description details because we have seperated the deal description details from the payments table. Please reference deal description details in 'PDeal_Descriptions' table. 

NB: We have not yet deleted the deal description details from the payments table though.

#Where to find the updated database
Please find and restore the current version of the tracker database from :
petratracker/Resources/db/DB Restore/Petra_Tracker_Bak_10_08_2015.

#Status on reports
About 70% of the reports have been completed. We need some help with some queries to be able to complete the remaining 30%. I will send the sql queries needed to pull those reports so we figure out how to convert it to LINQ queries.

#Todo in schedules
We appended a feild called ptas_fund_deal_id to the schedules table. We are required to fetch and store the fund_deal_id from the PTas when creatin a schedule.

#All well and done


# Introduction 
SailTally is a 100% donated custom developed application for the Minnetonka Yacht Club (MYC) one design sailboat racing, initially put into service for the 2010 season.  Significant development improvements have been completed over the years since then, including mobile responsive support.  The UX is old school, but effective.

One of the key benefits of SailTally is its support for multiple series being scored for a single fleet’s race.  If a fleet has a race and the scoring of that race needs to influence one or more series for the single race, the scorer only needs to enter in the results once.

While a scorer is entering results, visitors will still see the old results.  Once the results have been fully posted, they are published by the scorer, thereby calculating the scores and places, and made available via Display Results.  Display Results can be viewed anonymously, unlike many other areas of the application which requires security access.

*This documentation presumes a high level of technical expertise for installation and doesn’t currently include any documented details on its operation except via source code.*  It is hopeful that this changes in the future at some point, either because the application development has matured to completion and this is the last task to complete or other volunteers assist with the process, and potentially bring this project into the open source domain.  Currently, making development changes and trying to keep the documentation accurate and update to date is a significant task.

Whether you sail for pleasure or racing competitively, be sure to keep it happy.

# About the Authors
Hans Dickel is currently the IT Director of the Web Development Team at [Daikin Applied Americas](https://www.daikinapplied.com).  At the time this project was started, he was the founder of Aeriden LLC, a Microsoft partner focused on web development and software services on the .NET Platform and DevOps.  Having been an avid one-design sailor (C Scow, E Scow, Melges 17, A Scow, Sonar, and a mix of larger boats) and volunteer for the MYC (donating IT services, hosting, software development, former board member and flag officer), this project was developed as part of donating to the club.

# Database Schema
View the PDF document of the [DB Schema](/Setup/SailTally%20DB%20Schema.pdf).

# Installation
## Deployment Requirements

- .NET Framework 4.6.1
- One of the following databases:
    - Microsoft SQL Server 2012+ with sql authentication enabled or Integration Authentication using an IIS Application Pool account
    - Azure DB v12+ with sql authentication
- Internet information Server (IIS) 7.0+
- Visual Studio 2017+
- C#
- ASP.NET Webforms
- LINQ to sql via Entity Framework

## Pre-Installation Preparation
If upgrading an existing system, make sure to backup the website, database, and their settings as appropriate prior to performing the upgrade.

Make sure IIS is installed on the desired Operating System.

## Setup
Setup SailTally in a development environment to get an understanding on its configuration and to troubleshoot any issues before going to production.  It may be useful to do your initial configuration and then backup your SQL DB, restoring it to the production SQL server.

1. Create a new IIS website (e.g., SailTally).  This creates a new Application Pool (e.g., "IIS AppPool\SailTally").
2. Create a new SQL Server Database.
3. Add the Application Pool user to the new Database, granting one of the following security models:

    Grant this user db_owner access (not recommended for Production systems).

    Grant a SQL User (not recommended for Production systems) or Application Pool user:
    - aspnet_Membership_BasicAccess
    - aspnet_Membership_FullAccess
    - aspnet_Membership_ReportingAccess
    - db_datareader
    - db_datawriter    

4. Run the included SQL Script.

    For new installations, run the sql script *setup\SailTally_vX.Y_New.sql*, where X.Y is the actual version of sailTally being installed.

    For upgrades, locate the SQL script *setup\Sailtally_vX.Y_Upgrade_from_vZ.A.sql*, where Z.A is the current version installed and X.Y is hte new version being installed.

    Note that you may need to run more than one upgrade script to get up to the current released version.

5. Browse to the website.  The default SailTally username and password are as follows:
    - User: stadmin
    - Password: Sail27Fast!

    **It is recommended the password be changed before the site is publicly available.**

6. Publish the website using the *Release* configuration.  Replace the following "variables" with specific information:
    * $DBCONNSTR : SQL Server connection string
    * $TRACKID : Google Analytics Information
    
    This approach is useful for automating and securing deployments (such as via Azure Pipelines with Azure Vault).

# The Future
It is good to have dreams.

There is an eye to replace this solution with a ASP.NET Core 3.1 MVC (or Razor) version of the application.  Or hold out for .NET 5.

A redesigned UX is a must with support for mobile or tablet touch, while still support the mouse and keyboard.  The ultimate desirable library is TBD, although there is an eye towards Blazor for client-side interactions.

# Contributors
- [Hans Dickel](mailto:hans@raceh2o.com): primary development contributor.
- Blake Middleton: primary race officer (PRO) for the MYC, master scorer
- Gretchen Wilbrandt: MYC Executive Director, master scheduler
- Carol McGoldrick: former MYC club manager, master scheduler
- Pat Fleming: initial tester in 2009-2010
- Tom vergburt: initial project management in 2009-2010
- Jonathan McDonagh: initial design assistance

# Version History
**Version 1.2.0**
Moved to [GitHub](https://www.github.com) and performed updates to support Azure Pipelines CI/CD (Continuous Integration/Continuous Deployment).

Removed prior version history to start with cleaned up and simplified documentation.

**Version 1.1.9 and earlier**
Used CodePlex (now defunct and open source repo removed).  

~End~

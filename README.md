# Introduction 
SailTally is a 100% donated custom developed application for the Minnetonka Yacht Club (MYC) one design sailboat racing, initially put into service for the 2010 season.  Significant development improvements have been completed over the years since then.

One of the key benefits of SailTally is its support for multiple series being scored for a single fleet’s race.  If a fleet has a race and the scoring of that race needs to influence one or more series for the single race, the scorer only needs to enter or in the results once.

While a scorer is entering results, visitors will still see the old results.  Once the results have been fully posted, they are published by the scorer, thereby calculating the scores and places, and made available via Display Results.  Display Results can be viewed anonymously, unlike many other areas of the application which requires security access.

*This documentation presumes a high level of technical expertise for installation and doesn’t currently include any documented details on its operation except via source code.*  It is hopeful that this changes in the future at some point, either because the application development has matured to completion and this is the last task to complete or other volunteers assist with the process, and potentially bring this project into the open source domain.  Currently, making development changes and trying to keep the documentation accurate and update to date is a significant task.

Whether you sail for pleasure or racing competitively, be sure to keep it happy.

# About the Authors
Hans Dickel is currently the IT Director of the Web Development Team at [Daikin Applied Americas](www.daikinapplied.com).  At the time this project was started, he was the founder of Aeriden LLC, a Microsoft partner focused on web development and software services on the .NET Platform and DevOps.  Having been an avid one-design sailor (C Scow, E Scow, Melges 17, A Scow, Sonar, and a mix of larger boats) and volunteer for the MYC (donating IT services, hosting, software development, former board member and flag officer), this project was developed as part of donating to the club.

# Database Schema


# Installation
## Deployment Requirements

- .NET Framework 4.6.1
- One of the following databases:
    - Microsoft SQL Server 2012+ with sql authentication enabled or Integration Authentication using an IIS Applicaiton Pool account
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
Create a new SQL Server Database and run the included SQL Script.

For new installations, run the sql script *setup\SailTally_vX.Y New.sql*, where X.Y is the actual version of sailTally being installed.

For upgrades, locate the SQL script *setup\Sailtally_vX.Y Upgrade from Z.A.sql*, where Z.A is the current version installed and X.Y is hte new version being installed.

Note that you may need to run more than one upgrade script to get up to the current released version.

Create a new website (or subweb) with the appropriate .NET version.  This is operating system dependent, so specific instructions are not provided.

The SQL user or IIS AppPool user needs to be granted the following User Mappings:
- aspnet_Membership_BasicAccess
- aspnet_Membership_FullAccess
- aspnet_Membership_ReportingAccess
- db_datareader
- db_datawriter

An alternative is to grant this user db_owner access (not recommended for Production systems).

The default SailTally username and password are as follows:

- User: stadmin
- Password: SailFast

It is recommended the password be changed before the site is publicly available.

# Contributors
- [Hans Dickel](mailto:hans@raceh2o.com): primary development contributor.
- Blake Middleton: primary race officer (PRO) for the MYC, master scorer
- Gretchen Wilbrandt: MYC Executive Director, master scheduler
- Carol McGoldrick: former MYC club manager, master scheduler
- Pat Fleming: initial tester
- Tom vergburt: initial project management
- Jonathan McDonagh: initial design assistance


~End~

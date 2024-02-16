# Akka.net sqlite delay example
This is an example app to demonstrate the initial ~10s delay when persistence querying an Sqlite database mentioned on GitHub at https://github.com/akkadotnet/Akka.Persistence.Sql/issues/345.

I couldn't reproduce this at first but then tried to align this project as my main project was until I can reproduce the issue and then strip away everything that I don't need. The thing I found out was that the delay didn't occur until the ordering_id was higher.

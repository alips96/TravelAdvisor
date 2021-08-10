# TravelAdvisor
A real-time worldwide covid19 tracker app.

The whole idea of this app is to give information about the coronavirus situation and recommend safety precautions based on the destination and other places a preson intends to visit.

Here you can see the dataset on github. It contains details regarding the coronavirus situation of each country (in some larger countries like the US and Germany it is divided by state). Here is the link to the .csv file:

https://github.com/CSSEGISandData/COVID-19/blob/master/csse_covid_19_data/csse_covid_19_daily_reports/11-28-2020.csv

This file gets updated every day. So every day, the app has to access this file and get the data of the previous day.

The user can enter the starting point and multiple destinations he/she is planning to visit. The app extracts the data related to the places the visitor intends to go and calculates the risk of the travel. K-Means (A popular machine learning algorithm) is used in this process. Then based on the overall risk calculated in the previous part, it gives the overview of the coronavirus situation in each of those areas and subdivides the travel risk into different categories:

  . Green: the app wishes good luck to the user and gives them basic recommendations.
  
  . Yellow: it means traveling to those areas is a little risky and gives recommendations accordingly.
  
  . Orange: the app suggests more safety precautions and discourages the user to travel to those areas. 
  
  . Red: The app strongly discourages the user from traveling to those destinations and informs them about the highest safety precautions they should consider during their travel.
  
  Screenshots of the app is uploaded under screenshots folder.

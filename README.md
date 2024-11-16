# TravelAdvisor: Real-Time Worldwide COVID-19 Tracker App

**TravelAdvisor** is a real-time application designed to provide up-to-date information about the COVID-19 situation worldwide. It helps users make informed travel decisions by recommending safety precautions based on their intended destinations.

---

## Overview

The app offers the following key features:
- Access to daily updated COVID-19 data.
- Risk analysis for multiple travel destinations.
- Personalized travel safety recommendations based on the calculated risk level.

---

## Dataset

The app uses the COVID-19 dataset provided by [CSSEGISandData](https://github.com/CSSEGISandData/COVID-19).  
You can find the dataset here:  
[COVID-19 Daily Reports CSV File](https://github.com/CSSEGISandData/COVID-19/blob/master/csse_covid_19_data/csse_covid_19_daily_reports/11-28-2020.csv)

- **Details**:
  - The dataset includes information on the COVID-19 situation for each country.
  - For larger countries (e.g., the US, Germany), data is subdivided by state.
  - The file is updated daily, and the app retrieves data from the previous day to ensure accurate analysis.

---

## Functionality

1. **Input**:
   - Users enter a starting point and multiple destinations they plan to visit.
2. **Data Extraction**:
   - The app extracts relevant data for the specified locations.
3. **Risk Calculation**:
   - **K-Means Algorithm**: A machine learning algorithm is employed to calculate the overall travel risk.
4. **Risk Categorization**:
   - Based on the calculated risk, destinations are categorized into one of the following levels:
     - **Green**: Minimal risk. The app provides basic recommendations and wishes the user a safe journey.
     - **Yellow**: Moderate risk. The app suggests additional precautions.
     - **Orange**: High risk. The app discourages travel and recommends strict safety measures.
     - **Red**: Severe risk. Travel is strongly discouraged, and the app emphasizes maximum safety precautions.

---

## Screenshots

Screenshots of the app are available in the **`/screenshots`** folder.

---

This app combines real-time data analysis with machine learning to help users travel safely during the pandemic. If you have feedback or suggestions, feel free to contribute or reach out!

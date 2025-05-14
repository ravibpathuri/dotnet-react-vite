import { useEffect, useState } from "react";
import axios from "axios";

interface WeatherForecastData {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

export function WeatherForecast() {
  const [forecasts, setForecasts] = useState<WeatherForecastData[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    axios
      .get<WeatherForecastData[]>("api/Home/Get", { withCredentials: true })
      .then((response) => {
        setForecasts(response.data);
        setLoading(false);
      })
      .catch((error) => {
        console.error("Error fetching weather data:", error);
        setLoading(false);
      });
  }, []);

  return (
    <div className="weather-container">
      <h1>Weather Forecast</h1>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Temp. (C)</th>
              <th>Temp. (F)</th>
              <th>Summary</th>
            </tr>
          </thead>
          <tbody>
            {forecasts.map((forecast, index) => (
              <tr key={index}>
                <td>{forecast.date}</td>
                <td>{forecast.temperatureC}</td>
                <td>{forecast.temperatureF}</td>
                <td>{forecast.summary}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

# k6-loadtests
Demonstrates how to run load tests with containerised instances of K6, Grafana and InfluxDB.

### Dashboards
The dashboard in /dashboards is adapted from the excellent K6 / Grafana dashboard here:
https://grafana.com/grafana/dashboards/2587

### There are only two small modifications:
1) the data source is configured to use the docker created InfluxDB data source 
2) the time period is set to now-15m
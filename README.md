# Hendursaga

## 📌 Overview
**Hendursaga** is a **local-first** monitoring dashboard designed to oversee your **home network, local servers, and system performance**. Named after the Mesopotamian god **Hendursaga**, known as a divine night watchman and protector, this project embodies vigilance and guardianship over your infrastructure.

## 🚀 Features
### ✅ System Monitoring
- [ ] Collect **CPU, RAM, Disk, and Network** usage from the local system.
- [ ] Track **running processes and resource usage**.
- [ ] Monitor **active network connections** and bandwidth usage.

### ✅ Docker & Kubernetes Monitoring
- [ ] Display **running Docker containers** and their resource usage.
- [ ] Monitor **K3s pods and nodes** for status and resource consumption.
- [ ] Detect **container restarts, failures, and anomalies**.

### ✅ Log Aggregation & Analysis
- [ ] Use **Logstash to collect system logs** (syslog, application logs, network logs).
- [ ] Store logs in **Elasticsearch** and analyze them in **Kibana**.
- [ ] Detect and flag **critical errors and unusual log patterns**.

### ✅ Alerting & Notifications
- [ ] **Prometheus Alertmanager** triggers alerts based on custom thresholds.
- [ ] Send alerts via **Discord, Slack, or Email**.
- [ ] Custom notification rules for **high CPU usage, memory leaks, or container failures**.

### ✅ Web Dashboard (React + Tailwind + Grafana + Kibana)
- [ ] **Real-time graphs** for system performance (CPU, memory, network traffic).
- [ ] **Kibana integration** for searching logs.
- [ ] **Container & Kubernetes status overview**.
- [ ] Customizable widgets for **personalized monitoring**.

### ✅ Local-First & DevOps Automation
- [ ] **Runs entirely on a local system** without cloud dependencies.
- [ ] **Dockerized setup** for easy deployment.
- [ ] **K3s for lightweight Kubernetes orchestration**.
- [ ] **GitHub Actions** automates build and deployment.

## 🛠️ Tech Stack
### **Backend** (Golang)
- **Gorilla Mux** – Web API for system data.
- **Prometheus Go Client** – Metrics collection.
- **ElasticSearch Go Client** – Log aggregation.

### **Frontend** (Templ + TailwindCSS)
- **Templ** – Web dashboard.
- **TailwindCSS** – UI styling.
- **Grafana** – Visualization for metrics.
- **Kibana** – Log searching and analysis.

### **Monitoring & Logging**
- **Prometheus + Node Exporter** – System performance metrics.
- **ELK Stack (Elasticsearch, Logstash, Kibana)** – Logging and visualization.
- **Alertmanager** – Custom alerting system.

### **Deployment & DevOps**
- **Docker & K3s** – Container orchestration.
- **Helm** – Kubernetes package management.
- **GitHub Actions** – Automated CI/CD.

## 📌 Next Steps
1. [ ] **Set up K3s locally**.
2. [ ] **Deploy Golang backend** for system metrics.
3. [ ] **Install Prometheus & Node Exporter**.
4. [ ] **Set up ELK Stack**.
5. [ ] **Develop React frontend with Tailwind**.
6. [ ] **Containerize and automate deployment with GitHub Actions**.

---


### 📢 Want to contribute or improve the setup? Open an issue or submit a PR! 🚀



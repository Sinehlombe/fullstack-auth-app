#!/bin/bash
echo "Building Fullstack Auth App..."
echo "Stopping any running containers..."
docker-compose down
echo "Building and starting all services..."
docker-compose up --build -d
echo "Build complete! App is running at http://localhost"
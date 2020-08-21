cd src

docker build -t demo_mef -f DemoMef.CLI/Dockerfile .
docker run -i -t demo_mef --project-name demo_mef 
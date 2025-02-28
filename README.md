это текстовый блог - mvc(mvvm)

для запуска нужо подключение к бд posgreSq и vs studio
команда для подёема бд в докере:
docker run --hostname=aedffdc72692 --mac-address=02:42:ac:11:00:02 --env=POSTGRES_USER=me --env=POSTGRES_PASSWORD=1234 --env=POSTGRES_DB=my_db 
--env=PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/usr/lib/postgresql/16/bin --env=GOSU_VERSION=1.17 
--env=LANG=en_US.utf8 --env=PG_MAJOR=16 --env=PG_VERSION=16.4-1.pgdg120+1 --env=PGDATA=/var/lib/postgresql/data --volume=/var/lib/postgresql/data --network=bridge -p 5433:5432 --restart=no --runtime=runc -d postgres

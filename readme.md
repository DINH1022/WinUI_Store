To use this database you need to install docker golbal, knex and postgrec

npm init

npm install --save knex dotenv tedious

npm install pg




if you don't have container yet
comand to your terminal: 

docker run --name postgres -e POSTGRES_PASSWORD=<Your-password> -p <Your-port:Your-port> -d postgres

docker exec -it postgres psql -U postgres -c "CREATE DATABASE <Your-nameshop>" 

cp .env.example .env

npx knex migrate:latest

npx knex seed:run



if you have container and database empty

docker exec -i postgres psql -U postgres -d demoshoeshop < <your-file.sql>

npx knex migrate:latest

npx knex migrate:latest
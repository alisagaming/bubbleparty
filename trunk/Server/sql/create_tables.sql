create table players (
	player_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	device_id VARCHAR(50) UNIQUE KEY,
	facebook_id VARCHAR(50) UNIQUE KEY,
	first_name VARCHAR(50),
        last_name VARCHAR(50),
        
	device_model VARCHAR(100),
	device_type VARCHAR(50),
	operating_system VARCHAR(100),
	region VARCHAR(50),
	email VARCHAR(255) UNIQUE KEY,
	
	registration_date DATETIME,
        last_played_date DATETIME,
                

	coins_total INT default 0,
	coins_spent INT default 0,
        diamond_total INT default 0,
	diamond_spent INT default 0,
	lives_total INT default 0,		
	lives_spent INT default 0,
	
	bonus_star INT default 0,
	bonus_time INT default 0,
	bonus_fireball INT default 0,
	bonus_plazma INT default 0,
	
	total_score INT default 0,
	total_expiriens INT default 0
);

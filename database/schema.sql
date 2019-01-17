-- *************************************************************************************************
-- This script creates all of the database objects (tables, constraints, etc) for the database
-- *************************************************************************************************



--CREATE DATABASE MealDB
--DROP DATABASE MealDB

CREATE TABLE Users(

user_id			int				identity(1,1),
firstname		varchar(64)		NOT NULL,
lastname		varchar(64)		NOT NULL,
username		varchar(64)		NOT NULL,
email			varchar(120)	NOT NULL,
hash			varchar(64)		NOT NULL,
salt			varchar(64)		NOT NULL

PRIMARY KEY (user_id)
)

CREATE TABLE Recipes(

recipe_id		int				identity(1,1),
recipe_name		varchar(64)		NOT NULL,
description		varchar(300)	NOT NULL,
instructions	varchar(5000)	NOT NULL,
cook_time		int				NOT NULL,
prep_time		int				NOT NULL,
creator_id		int				NOT NULL

PRIMARY KEY (recipe_id)
)

CREATE TABLE Images(
recipe_id		int				NOT NULL,
image_byte		varbinary(max)	

FOREIGN KEY (recipe_id) REFERENCES Recipes(recipe_id)
)

CREATE TABLE Ingredients(

ingredient_id		int				identity(1,1),
ingredient_name		varchar(64)		NOT NULL,
user_id				int,				

PRIMARY KEY (ingredient_id),
FOREIGN KEY (user_id) REFERENCES Users(user_id)
)

CREATE TABLE UsersRecipes(

user_id		int				NOT NULL,
recipe_id	int				NOT NULL

FOREIGN KEY (user_id) REFERENCES Users(user_id),
FOREIGN KEY (recipe_id) REFERENCES Recipes(recipe_id)
)

CREATE TABLE Units(

unit_id		int				identity(1,1),
unit_name	varchar(64)		CHECK (unit_name='tsp' OR unit_name='tbsp' OR unit_name='cup'
							OR unit_name='oz' OR unit_name='lb')

PRIMARY KEY (unit_id)
)

CREATE TABLE RecipesIngredients(

recipe_id		int				NOT NULL,
ingredient_id	int				NOT NULL,
notes			varchar(50),
quantity		varchar(20)		NOT NULL,
unit_id			int				NULL

FOREIGN KEY (recipe_id) REFERENCES Recipes(recipe_id),
FOREIGN KEY (ingredient_id) REFERENCES Ingredients(ingredient_id),
FOREIGN KEY (unit_id) REFERENCES Units(unit_id)
)

CREATE TABLE Category(

category_id			int				identity(1,1),
category_name		varchar(10)		CHECK (category_name='Breakfast' OR 
											category_name='Lunch' OR
											category_name='Dinner' OR
											category_name='Snack' OR
											category_name='Dessert')

PRIMARY KEY (category_id)
)

CREATE TABLE Meals(

meal_id				int				identity(1,1),
meal_name			varchar(64)		NOT NULL,
meal_description	varchar(120)	NULL,
user_id				int				NOT NULL,
category_id			int				NOT NULL,
creator_id			int				NOT NULL

PRIMARY KEY (meal_id),
FOREIGN KEY (user_id) REFERENCES Users(user_id),
FOREIGN KEY (category_id) REFERENCES Category(category_id)
)

CREATE TABLE Plans(

plan_id		int				identity(1,1),
plan_name	varchar(64)		NOT NULL,
user_id		int				NOT NULL

PRIMARY KEY (plan_id),
FOREIGN KEY (user_id) REFERENCES Users(user_id)
)

CREATE TABLE MealsRecipes(

meal_id		int		NOT NULL,
recipe_id	int		NOT NULL,

FOREIGN KEY (meal_id) REFERENCES Meals(meal_id),
FOREIGN KEY (recipe_id) REFERENCES Recipes(recipe_id)
)

CREATE TABLE MealsPlans(

meal_id		int		NOT NULL,
plan_id		int		NOT NULL,
day_id		int		NOT NULL

FOREIGN KEY (meal_id) REFERENCES Meals(meal_id),
FOREIGN KEY (plan_id) REFERENCES Plans(plan_id)
)

INSERT INTO Category (category_name)
VALUES ('Breakfast')

INSERT INTO Category (category_name)
VALUES ('Lunch')

INSERT INTO Category (category_name)
VALUES ('Dinner')

INSERT INTO Category (category_name)
VALUES ('Snack')

INSERT INTO Category (category_name)
VALUES ('Dessert')


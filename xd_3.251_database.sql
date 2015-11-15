/*
Navicat MySQL Data Transfer

Source Server         : 121.40.88.117
Source Server Version : 50537
Source Host           : 121.40.88.117:3306
Source Database       : xd_data

Target Server Type    : MYSQL
Target Server Version : 50537
File Encoding         : 936

Date: 2015-05-16 13:29:16
*/

SET FOREIGN_KEY_CHECKS=0;

DROP DATABASE  IF EXISTS  `xd_data`;
CREATE DATABASE `xd_data` CHARACTER SET gbk COLLATE gbk_bin;
grant all privileges on *.* to rootcdz@* identified by 'rootcdz';

use `xd_data`;
-- ----------------------------
-- Table structure for black_card_list_t
-- ----------------------------
DROP TABLE IF EXISTS `black_card_list_t`;
CREATE TABLE `black_card_list_t` (
`phy_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`user_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`register_time`  datetime NULL DEFAULT NULL ,
`start_black`  datetime NULL DEFAULT NULL ,
PRIMARY KEY (`phy_id`, `user_id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin

;

-- ----------------------------
-- Records of black_card_list_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for charge_mode_t
-- ----------------------------
DROP TABLE IF EXISTS `charge_mode_t`;
CREATE TABLE `charge_mode_t` (
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`charge_mode`  varbinary(8) NULL DEFAULT 0 ,
`effect_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`end_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`effect_state`  smallint(6) NULL DEFAULT 0 ,
`calcu_type`  smallint(6) NULL DEFAULT 0 ,
`charge_rate`  tinyint(4) NULL DEFAULT 0 ,
`time_nums`  tinyint(4) NULL DEFAULT 0 ,
`time1_para`  time NULL DEFAULT '00:00:00' ,
`time2_para`  time NULL DEFAULT '00:00:00' ,
`time3_para`  time NULL DEFAULT '00:00:00' ,
`time4_para`  time NULL DEFAULT '00:00:00' ,
`time5_para`  time NULL DEFAULT '00:00:00' ,
`time6_para`  time NULL DEFAULT '00:00:00' ,
`time7_para`  time NULL DEFAULT '00:00:00' ,
`time8_para`  time NULL DEFAULT '00:00:00' ,
`time9_para`  time NULL DEFAULT '00:00:00' ,
`time10_para`  time NULL DEFAULT '00:00:00' ,
`time11_para`  time NULL DEFAULT '00:00:00' ,
`time12_para`  time NULL DEFAULT '00:00:00' ,
PRIMARY KEY (`dev_id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin

;

-- ----------------------------
-- Records of charge_mode_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for common_data_t
-- ----------------------------
DROP TABLE IF EXISTS `common_data_t`;
CREATE TABLE `common_data_t` (
`sn_id`  int(11) NOT NULL AUTO_INCREMENT ,
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`user_id`  varchar(32) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`recv_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`raw_data`  varchar(2048) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT NULL ,
PRIMARY KEY (`sn_id`, `dev_id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin
AUTO_INCREMENT=1

;

-- ----------------------------
-- Records of common_data_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for dev_tree_t
-- ----------------------------
DROP TABLE IF EXISTS `dev_tree_t`;
CREATE TABLE `dev_tree_t` (
`node_id`  int(11) NOT NULL AUTO_INCREMENT ,
`node_name`  varchar(48) CHARACTER SET gbk COLLATE gbk_bin NULL DEFAULT NULL ,
`parent_id`  int(11) NULL DEFAULT NULL ,
`level`  int(11) NULL DEFAULT NULL ,
PRIMARY KEY (`node_id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin
AUTO_INCREMENT=17

;

-- ----------------------------
-- Records of dev_tree_t
-- ----------------------------
BEGIN;
INSERT INTO `dev_tree_t` VALUES ('1', '上海市', '0', '1'), ('2', '徐汇区', '1', '2'), ('3', '嘉定区', '1', '2'), ('4', '松江区', '1', '2'), ('6', '松江镇', '4', '3'), ('7', '金玉路永丰小区', '6', '4'), ('8', '虹桥镇', '2', '3'), ('9', '嘉定镇', '3', '3'), ('10', '万达广场', '9', '4'), ('11', '吴中路永达奥迪4S店', '8', '4'), ('12', '北京市', '0', '1'), ('13', '安徽省', '0', '1'), ('14', '合肥市', '13', '2'), ('15', '万达', '14', '3'), ('16', '万达小区1号停车场', '15', '4');
COMMIT;

-- ----------------------------
-- Table structure for hd_pile_info_t
-- ----------------------------
DROP TABLE IF EXISTS `hd_pile_info_t`;
CREATE TABLE `hd_pile_info_t` (
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`user_id`  varchar(32) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`vender_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`software_ver`  varchar(20) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`pile_type`  tinyint(4) NULL DEFAULT 2 ,
`sim_id`  varchar(11) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`address`  tinytext CHARACTER SET gbk COLLATE gbk_chinese_ci NULL ,
`position`  tinytext CHARACTER SET gbk COLLATE gbk_chinese_ci NULL ,
`date_time`  datetime NULL DEFAULT NULL ,
`node_id`  int(11) NULL DEFAULT NULL ,
PRIMARY KEY (`dev_id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin

;

-- ----------------------------
-- Records of hd_pile_info_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for money_change_info_t
-- ----------------------------
DROP TABLE IF EXISTS `money_change_info_t`;
CREATE TABLE `money_change_info_t` (
`id`  int(11) NOT NULL AUTO_INCREMENT ,
`phy_card`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`user_card_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NOT NULL DEFAULT '0' ,
`total_balance`  decimal(20,2) NOT NULL DEFAULT 0.00 ,
`account_balance`  decimal(20,2) NULL DEFAULT 0.00 ,
`elec_pkg_balance`  decimal(20,2) NULL DEFAULT 0.00 ,
`change_money`  decimal(20,2) NOT NULL ,
`type`  int(11) NOT NULL ,
`time`  varchar(20) CHARACTER SET gbk COLLATE gbk_bin NOT NULL DEFAULT '0000-00-00 00:00:00' ,
`manager_id`  int(11) NULL DEFAULT NULL ,
`manager_name`  varchar(32) CHARACTER SET gbk COLLATE gbk_bin NULL DEFAULT NULL ,
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NULL DEFAULT NULL ,
`charge_sn`  varchar(32) CHARACTER SET gbk COLLATE gbk_bin NULL DEFAULT NULL ,
PRIMARY KEY (`id`),
INDEX `fk_cardId` (`phy_card`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin
AUTO_INCREMENT=1

;

-- ----------------------------
-- Records of money_change_info_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for pile_black_list_t
-- ----------------------------
DROP TABLE IF EXISTS `pile_black_list_t`;
CREATE TABLE `pile_black_list_t` (
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`province_id`  tinyint(4) NULL DEFAULT 0 ,
`time_stamp`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`update_sn`  tinyint(4) NULL DEFAULT 0 ,
PRIMARY KEY (`dev_id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin

;

-- ----------------------------
-- Records of pile_black_list_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for pile_info_list_t
-- ----------------------------
DROP TABLE IF EXISTS `pile_info_list_t`;
CREATE TABLE `pile_info_list_t` (
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`pile_type`  tinyint(4) NULL DEFAULT 1 ,
`ac_input_vol_u`  decimal(6,1) NULL DEFAULT 0.0 ,
`ac_input_vol_v`  decimal(6,1) NULL DEFAULT 0.0 ,
`ac_input_vol_w`  decimal(6,1) NULL DEFAULT 0.0 ,
`ac_output_vol_u`  decimal(6,1) NULL DEFAULT 0.0 ,
`ac_output_vol_v`  decimal(6,1) NULL DEFAULT 0.0 ,
`ac_output_vol_w`  decimal(6,1) NULL DEFAULT 0.0 ,
`ac_output_cur`  decimal(6,2) NULL DEFAULT 0.00 ,
`dc_output_vol`  decimal(6,1) NULL DEFAULT 0.0 ,
`dc_output_cur`  decimal(6,2) NULL DEFAULT 0.00 ,
`total_degree`  decimal(10,1) NULL DEFAULT 0.0 ,
`taper_degree`  decimal(10,1) NULL DEFAULT 0.0 ,
`peak_degree`  decimal(10,1) NULL DEFAULT 0.0 ,
`flat_degree`  decimal(10,1) NULL DEFAULT 0.0 ,
`valley_degree`  decimal(10,1) NULL DEFAULT 0.0 ,
`date_time`  datetime NULL DEFAULT NULL ,
PRIMARY KEY (`dev_id`),
INDEX `index_time` (`date_time`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin

;

-- ----------------------------
-- Records of pile_info_list_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for pile_offline_records_t
-- ----------------------------
DROP TABLE IF EXISTS `pile_offline_records_t`;
CREATE TABLE `pile_offline_records_t` (
`sn_id`  int(11) NOT NULL AUTO_INCREMENT ,
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`user_id`  varchar(32) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`serial_sn`  varchar(32) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`phy_card`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`user_card`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`time_charge_type`  smallint(6) NULL DEFAULT 0 ,
`start_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`end_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`taper_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`taper_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`peak_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`peak_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`flat_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`flat_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`valley_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`valley_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`calcu_type`  smallint(6) NULL DEFAULT 0 ,
`total_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`total_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`taper_price`  decimal(15,5) NULL DEFAULT 0.00000 ,
`taper_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`taper_money`  decimal(10,2) NULL DEFAULT 0.00 ,
`peak_price`  decimal(15,5) NULL DEFAULT 0.00000 ,
`peak_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`peak_money`  decimal(10,2) NULL DEFAULT 0.00 ,
`flat_price`  decimal(15,5) NULL DEFAULT 0.00000 ,
`flat_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`flat_money`  decimal(10,2) NULL DEFAULT 0.00 ,
`valley_price`  decimal(15,5) NULL DEFAULT 0.00000 ,
`valley_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`valley_money`  decimal(10,2) NULL DEFAULT 0.00 ,
`total_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`business_type`  smallint(6) NULL DEFAULT 1 ,
`elec_wallet_ballance`  decimal(10,2) NULL DEFAULT 0.00 ,
`spend_price`  int(11) NULL DEFAULT 0 ,
`spend_money`  decimal(10,2) NULL DEFAULT 0.00 ,
`exch_id`  tinyint(4) NULL DEFAULT 3 ,
`client_id`  varbinary(6) NULL DEFAULT 0 ,
`wallet_ballance`  int(11) NULL DEFAULT 0 ,
`wallet_serial`  smallint(6) NULL DEFAULT 0 ,
`exch_money`  int(11) NULL DEFAULT 0 ,
`exch_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`srand_num`  int(11) NULL DEFAULT 0 ,
`exch_type`  tinyint(4) NULL DEFAULT 0 ,
`tac`  int(11) NULL DEFAULT 0 ,
`key_ver`  tinyint(4) NULL DEFAULT 0 ,
`client_exch_sn`  int(11) NULL DEFAULT 0 ,
PRIMARY KEY (`sn_id`, `dev_id`),
UNIQUE INDEX `index_serial_sn` (`serial_sn`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin
AUTO_INCREMENT=1

;

-- ----------------------------
-- Records of pile_offline_records_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for pile_online_records_t
-- ----------------------------
DROP TABLE IF EXISTS `pile_online_records_t`;
CREATE TABLE `pile_online_records_t` (
`sn_id`  int(11) NOT NULL AUTO_INCREMENT ,
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`user_id`  varchar(32) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`serial_sn`  varchar(32) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`business_type`  smallint(6) NULL DEFAULT 1 ,
`phy_card`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`user_card`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`start_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`end_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`taper_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`taper_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`peak_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`peak_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`flat_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`flat_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`valley_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`valley_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`taper_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`peak_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`flat_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`valley_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`total_pwr`  decimal(10,2) NULL DEFAULT 0.00 ,
`calcu_type`  smallint(6) NULL DEFAULT 2 ,
`total_start_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`total_end_val`  decimal(10,2) NULL DEFAULT 0.00 ,
`pre_charge_dev`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`time_charge_type`  smallint(6) NULL DEFAULT 0 ,
`veh_id`  varchar(32) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
PRIMARY KEY (`sn_id`, `dev_id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin
AUTO_INCREMENT=1

;

-- ----------------------------
-- Records of pile_online_records_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for pile_state_t
-- ----------------------------
DROP TABLE IF EXISTS `pile_state_t`;
CREATE TABLE `pile_state_t` (
`id`  int(11) NOT NULL AUTO_INCREMENT ,
`dev_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_bin NOT NULL ,
`vender_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`city_num`  varchar(8) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`software_ver`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`output_vol`  decimal(6,1) NULL DEFAULT 0.0 ,
`output_cur`  decimal(6,1) NULL DEFAULT 0.0 ,
`relay_state`  tinyint(4) NULL DEFAULT 0 ,
`conn_state`  tinyint(4) NULL DEFAULT 0 ,
`total_degree`  decimal(10,1) NULL DEFAULT 0.0 ,
`battery`  tinyint(4) NULL DEFAULT 0 ,
`work_state`  smallint(6) NULL DEFAULT 0 ,
`date_time`  datetime NULL DEFAULT NULL ,
PRIMARY KEY (`id`),
INDEX `index_time` (`date_time`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin
AUTO_INCREMENT=1

;

-- ----------------------------
-- Records of pile_state_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for user_card_list_t
-- ----------------------------
DROP TABLE IF EXISTS `user_card_list_t`;
CREATE TABLE `user_card_list_t` (
`id`  int(11) NOT NULL AUTO_INCREMENT ,
`phy_card`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NOT NULL ,
`user_card_id`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NOT NULL DEFAULT '0' ,
`card_passwd`  varchar(6) CHARACTER SET gbk COLLATE gbk_bin NULL DEFAULT NULL ,
`master_name`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`sex`  tinyint(4) NULL DEFAULT 0 ,
`phone_num`  varchar(12) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`id_type`  tinyint(4) NULL DEFAULT 0 ,
`id_num`  varchar(18) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`license_plate`  varchar(8) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`home_addr`  varchar(128) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`mail_addr`  varchar(64) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`start_time`  datetime NULL DEFAULT '0000-00-00 00:00:00' ,
`card_state`  tinyint(4) NULL DEFAULT 1 ,
`total_balance`  decimal(20,2) NULL DEFAULT NULL ,
`account_balance`  decimal(20,2) NULL DEFAULT 0.00 ,
`elec_pkg_balance`  decimal(20,2) NULL DEFAULT 0.00 ,
PRIMARY KEY (`id`, `phy_card`, `user_card_id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin
AUTO_INCREMENT=1

;

-- ----------------------------
-- Records of user_card_list_t
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for user_manage_t
-- ----------------------------
DROP TABLE IF EXISTS `user_manage_t`;
CREATE TABLE `user_manage_t` (
`id`  int(11) NOT NULL AUTO_INCREMENT ,
`user_name`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT NULL ,
`passwd`  varchar(16) CHARACTER SET gbk COLLATE gbk_chinese_ci NULL DEFAULT '0' ,
`user_auth`  tinyint(4) NULL DEFAULT 3 ,
`reg_time`  varchar(20) CHARACTER SET gbk COLLATE gbk_bin NULL DEFAULT '0000-00-00 00:00:00' ,
PRIMARY KEY (`id`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=gbk COLLATE=gbk_bin
AUTO_INCREMENT=11

;

-- ----------------------------
-- Records of user_manage_t
-- ----------------------------
BEGIN;
INSERT INTO `user_manage_t` VALUES ('1', 'admin', '123456', '1', '0000-00-00 00:00:00');
COMMIT;

-- ----------------------------
-- Table structure for xd_config
-- ----------------------------
DROP TABLE IF EXISTS `xd_config`;
CREATE TABLE `xd_config` (
`id`  int(11) NOT NULL AUTO_INCREMENT ,
`type`  varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL ,
`value`  varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL ,
PRIMARY KEY (`id`),
UNIQUE INDEX `xd_config_type_key` (`type`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci
AUTO_INCREMENT=7

;

-- ----------------------------
-- Records of xd_config
-- ----------------------------
BEGIN;
INSERT INTO `xd_config` VALUES ('1', 'block1Key', '88683A58DC9B7877886988683A58DC9C'), ('2', 'block16key', '8868B72438397B4788698868B724383A'), ('3', 'block32key', '88689F54DA186A57896988689F54DA19'), ('4', 'tagMode', '1'), ('5', 'tcpip', '192.168.3.250'), ('6', 'tcpport', '5188');
COMMIT;

-- ----------------------------
-- Auto increment value for common_data_t
-- ----------------------------
ALTER TABLE `common_data_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for dev_tree_t
-- ----------------------------
ALTER TABLE `dev_tree_t` AUTO_INCREMENT=17;

-- ----------------------------
-- Auto increment value for money_change_info_t
-- ----------------------------
ALTER TABLE `money_change_info_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for pile_offline_records_t
-- ----------------------------
ALTER TABLE `pile_offline_records_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for pile_online_records_t
-- ----------------------------
ALTER TABLE `pile_online_records_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for pile_state_t
-- ----------------------------
ALTER TABLE `pile_state_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for user_card_list_t
-- ----------------------------
ALTER TABLE `user_card_list_t` AUTO_INCREMENT=1;

-- ----------------------------
-- Auto increment value for user_manage_t
-- ----------------------------
ALTER TABLE `user_manage_t` AUTO_INCREMENT=11;

-- ----------------------------
-- Auto increment value for xd_config
-- ----------------------------
ALTER TABLE `xd_config` AUTO_INCREMENT=7;

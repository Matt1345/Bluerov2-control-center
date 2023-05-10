#!/usr/bin/env python3

import rospy
import random
from geometry_msgs.msg import Quaternion, Vector3
from sensor_msgs.msg import Imu

def imu_publisher():
    rospy.init_node('imu_publisher', anonymous=True)
    imu_pub = rospy.Publisher('/mavros/imu/data', Imu, queue_size=10)
    rate = rospy.Rate(10) # 10Hz

    while not rospy.is_shutdown():
        imu_msg = Imu()
        imu_msg.header.stamp = rospy.Time.now()
        imu_msg.header.frame_id = 'base_link'

        # Angular Velocity
        imu_msg.angular_velocity = Vector3()
        imu_msg.angular_velocity.x = random.uniform(-1, 1)
        imu_msg.angular_velocity.y = random.uniform(-1, 1)
        imu_msg.angular_velocity.z = random.uniform(-1, 1)
        imu_msg.angular_velocity_covariance = [0.0] * 9

        # Linear Acceleration
        imu_msg.linear_acceleration = Vector3()
        imu_msg.linear_acceleration.x = random.uniform(-10, 10)
        imu_msg.linear_acceleration.y = random.uniform(-10, 10)
        imu_msg.linear_acceleration.z = random.uniform(-10, 10)
        imu_msg.linear_acceleration_covariance = [0.0] * 9

        # Orientation (Quaternion)
        imu_msg.orientation = Quaternion()
        rotation = Quaternion()
        rotation.x = 0
        rotation.y = random.uniform(-1, 1) * 2 * 3.14159265  # Full rotation around y-axis
        rotation.z = 0
        rotation.w = 1
        imu_msg.orientation = rotation
        imu_msg.orientation_covariance = [-1.0] + [0.0] * 8

        imu_pub.publish(imu_msg)
        rate.sleep()

if __name__ == '__main__':
    try:
        imu_publisher()
    except rospy.ROSInterruptException:
        pass


﻿using System.Collections;

public abstract class PhysicsSkill : Skill {
	public float attackBouns; 

	public PhysicsSkill(string skillName, SkillType type, float attackBouns) : base(skillName, type) 
	{
		this.attackBouns = attackBouns;
	}
}

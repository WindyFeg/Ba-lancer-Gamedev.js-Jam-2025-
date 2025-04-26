using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

// using AnimationState = Spine.AnimationState;

public class ModelSpine : MonoBehaviour, IModelSpine
{
    Coroutine cur_coroutine;
    bool isAttacking = false;

    [SerializeField]
    SkeletonAnimation skeleton_animation;
    Spine.AnimationState _cur_state;
    Spine.Skeleton _skeleton;

    [SerializeField][SpineAnimation] string up_idle_anim;
    [SerializeField][SpineAnimation] string move_up_anim;
    [SerializeField][SpineAnimation] string side_idle_anim;
    [SerializeField][SpineAnimation] string mode_side_anim;
    [SerializeField][SpineAnimation] string down_idle_anim;
    [SerializeField][SpineAnimation] string move_down_anim;
    [SerializeField][SpineAnimation]  public string attack_anim;
    [SerializeField][SpineAnimation] string hit_anim;
    [SerializeField][SpineAnimation] public string death_anim;

    string currentAnimName;

    void Awake()
    {
        _cur_state = skeleton_animation.AnimationState;
        _skeleton = skeleton_animation.Skeleton;
    }

    //----------- ANIMATIONS ------------

    public void up_idle_playing() => play_animation_if_new(up_idle_anim, true);
    public void move_up_playing() => play_animation_if_new(move_up_anim, true);
    public void side_idle_playing() => play_animation_if_new(side_idle_anim, true);
    public void move_side_playing() => play_animation_if_new(mode_side_anim, true);
    public void down_idle_playing() => play_animation_if_new(down_idle_anim, true);
    public void move_down_playing() => play_animation_if_new(move_down_anim, true);

    public void attack_start()
    {
        if (isAttacking) return; // avoid overlap
        isAttacking = true;
        cur_coroutine = StartCoroutine(play_attack_cr());
    }

    public void hit_start()
    {
        if (isAttacking) return; // avoid overlap
        cur_coroutine = StartCoroutine(play_hit_cr());
    }

    public void death_start()
    {
        StartCoroutine(play_death_cr());
    }
    IEnumerator play_death_cr()
    {
        _cur_state.SetAnimation(0, death_anim, false).MixDuration = 0;
        yield return new WaitForSeconds(get_duration(death_anim));
        _cur_state.SetAnimation(0, down_idle_anim, false).MixDuration = 0;
    }

    IEnumerator play_hit_cr()
    {
        _cur_state.SetAnimation(0, hit_anim, false).MixDuration = 0;
        yield return new WaitForSeconds(get_duration(hit_anim));
        _cur_state.SetAnimation(0, down_idle_anim, true).MixDuration = 0;
    }

    IEnumerator play_attack_cr()
    {
        _cur_state.SetAnimation(0, attack_anim, false).MixDuration = 0;
        yield return new WaitForSeconds(get_duration(attack_anim));
        isAttacking = false; // attack done

        // Optionally return to idle or side move
        _cur_state.SetAnimation(0, mode_side_anim, true).MixDuration = 0;
        currentAnimName = mode_side_anim;
    }

    //------------- HELPERS -------------

    void stop_coroutine()
    {
        if (cur_coroutine != null)
        {
            StopCoroutine(cur_coroutine);
            cur_coroutine = null;
        }
    }

    public void direction(int direction)
    {
        direction = direction > 0 ? 1 : -1;
        _skeleton.ScaleX = direction;
    }

    public string get_name()
    {
        return _skeleton.Skin.Name;
    }

    public float get_duration(string anim_name)
    {
        var anim_obj = skeleton_animation
            .skeletonDataAsset.GetSkeletonData(false)
            .FindAnimation(anim_name);

        return anim_obj?.Duration ?? 0;
    }

    void play_animation_if_new(string animName, bool loop)
    {
        if (animName == death_anim) 
        {
            stop_coroutine(); // stop any current animation
            _cur_state.SetAnimation(0, animName, false).MixDuration = 0;
            currentAnimName = animName;
            return;
        }
        if (isAttacking) return; // block while attacking
        if (currentAnimName == animName) return;

        stop_coroutine();
        _cur_state.SetAnimation(0, animName, loop).MixDuration = 0;
        currentAnimName = animName;
    }
}

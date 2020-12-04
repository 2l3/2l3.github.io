<?php

namespace App\Transformers;

use App\Models\Setting;
use Flugg\Responder\Transformers\Transformer;

class SettingTransformer extends Transformer
{
    /**
     * List of available relations.
     *
     * @var string[]
     */
    protected $relations = [];

    /**
     * List of autoloaded default relations.
     *
     * @var array
     */
    protected $load = [];

    /**
     * Transform the model.
     *
     * @param  Setting  $setting
     * @return array
     */
    public function transform(Setting $setting)
    {
        return [
            'key' => (string) $setting->key,
            'value' => $setting->decodeValue(),
        ];
    }
}
